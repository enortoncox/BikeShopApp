using AutoMapper;
using BikeShopApp.Core.Models;
using BikeShopApp.Infrastructure.DatabaseContext;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using BikeShopApp.Core.Identity;

namespace BikeShopApp.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateOrderAsync(Order order, ICollection<int> productsIds)
        {
            var newOrder = _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var productId in productsIds)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

                if (product == null) 
                {
                    continue;
                }

                var ordersProductsEntity = new OrdersProducts
                {
                    Order = order,
                    Product = product
                };

                _context.OrdersProducts.Add(ordersProductsEntity);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var ordersProductsEntities = await _context.OrdersProducts.Where(op => op.OrderId == orderId).ToListAsync();
            _context.OrdersProducts.RemoveRange(ordersProductsEntities);

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<List<Order>> GetAllOrdersFromAfterPeriodAsync(DateTime date)
        {
            return _context.Orders.Where(o => o.OrderedDate >= date).ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsInOrderAsync(int orderId)
        {
            var ordersProductsEntity = await _context.OrdersProducts.Where(op => op.OrderId == orderId).ToListAsync();

            var products = new List<Product>();

            foreach (var item in ordersProductsEntity)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                if (product != null)
                {
                    products.Add(product);
                }
            }

            return products;
        }

        public Task<Order?> GetOrderAsync(int orderId)
        {
            return _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public Task<List<Order>> GetOrdersAsync()
        {
            return _context.Orders.ToListAsync();
        }

        public Task<List<Order>> GetOrdersFromUserAsync(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<OrdersPageResponseDto?> GetOrdersFromUserByPageAsync(int userId, string currentPage, string pageResults)
        {
            if (currentPage != null && currentPage != "" && int.TryParse(currentPage, out int pageParsed))
            {
                if (pageResults != null && pageResults != "" && float.TryParse(pageResults, out float resultsParsed))
                {
                    var userOrders = await GetOrdersFromUserAsync(userId);

                    var pageCount = Math.Ceiling(userOrders.Count() / resultsParsed);

                    var orders = userOrders
                        .Skip((pageParsed - 1) * (int)resultsParsed)
                        .Take((int)resultsParsed)
                        .ToList();

                    var ordersMapped = _mapper.Map<List<OrderDto>>(orders);

                    OrdersPageResponseDto response = new OrdersPageResponseDto()
                    {
                        Orders = ordersMapped,
                        CurrentPage = pageParsed,
                        Pages = (int)pageCount
                    };

                    return response;
                }
            }

            return null;
        }

        public async Task<ApplicationUser?> GetUserOfOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) 
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == order.UserId);

            return user;
        }

        public Task<bool> OrderExistsAsync(int orderId)
        {
            return _context.Orders.AnyAsync(o => o.OrderId == orderId);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
