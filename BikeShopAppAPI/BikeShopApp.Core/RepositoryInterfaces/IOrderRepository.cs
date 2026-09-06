using BikeShopApp.Core.Models;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.Identity;

namespace BikeShopApp.Core.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(int orderId);
        Task<List<Order>> GetOrdersAsync();
        Task<bool> CreateOrderAsync(Order order, ICollection<int> productsIds);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<ApplicationUser?> GetUserOfOrderAsync(int orderId);
        Task<List<Order>> GetOrdersFromUserAsync(int userId);
        Task<OrdersPageResponseDto?> GetOrdersFromUserByPageAsync(int userId, string currentPage, string pageResults);
        Task<List<Product>> GetAllProductsInOrderAsync(int orderId);
        Task<List<Order>> GetAllOrdersFromAfterPeriodAsync(DateTime date);
        Task<bool> OrderExistsAsync(int orderId);
    }
}
