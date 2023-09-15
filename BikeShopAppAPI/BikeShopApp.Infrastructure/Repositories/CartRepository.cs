using BikeShopApp.Core.Models;
using BikeShopApp.Infrastructure.DatabaseContext;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeShopApp.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToCartAsync(int cartId, int productId)
        {
            var cart = await GetCartAsync(cartId);
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (cart != null && product != null)
            {
                var cartsProductsEntity = new CartsProducts
                {
                    Cart = cart,
                    Product = product
                };

                _context.CartsProducts.Add(cartsProductsEntity);

                cart.TotalQuantity++;
                cart.TotalCost += product.Price;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> CartExistsAsync(int cartId)
        {
            return _context.Carts.AnyAsync(c => c.CartId == cartId);
        }

        public async Task<bool> CreateCartAsync(int userId)
        {
            var cart = new Cart()
            {
                TotalCost = 0,
                TotalQuantity = 0,
                UserId = userId
            };

            _context.Carts.Add(cart);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EmptyCartAsync(int cartId)
        {
            var cartsProductsEntities = await _context.CartsProducts.Where(cp => cp.CartId == cartId).ToListAsync();
            _context.CartsProducts.RemoveRange(cartsProductsEntities);

            var cart = await GetCartAsync(cartId);

            if (cart != null)
            {
                cart.TotalCost = 0;
                cart.TotalQuantity = 0;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<Cart?> GetCartAsync(int cartId)
        {
            return _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        public async Task<int> GetNumberOfItemsInCartAsync(int cartId)
        {
            var cart = await GetCartAsync(cartId);

            if (cart != null)
            {
                return cart.TotalQuantity;
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<Product>> GetProductsInCartAsync(int cartId)
        {
            var cartsProductsEntities = await _context.CartsProducts.Where(cp => cp.CartId == cartId).Include(p => p.Product).ToListAsync();

            List<Product> products = new List<Product>();

            foreach (var item in cartsProductsEntities)
            {
                products.Add(item.Product);
            }

            return products;
        }

        public async Task<decimal> GetTotalPriceOfCartAsync(int cartId)
        {
            var cart = await GetCartAsync(cartId);

            if (cart != null)
            {
                return cart.TotalCost;
            }
            else
            {
                return -1;
            }
        }

        public async Task<bool> RemoveProductFromCartAsync(int cartId, int productId)
        {
            var cartsProductsEntity = await _context.CartsProducts.FirstOrDefaultAsync(cp => cp.ProductId == productId && cp.CartId == cartId);
            _context.CartsProducts.Remove(cartsProductsEntity);

            var cart = await GetCartAsync(cartId);
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (cart != null && product != null)
            {
                cart.TotalQuantity--;
                cart.TotalCost -= product.Price;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
