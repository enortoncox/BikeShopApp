using BikeShopApp.Dto;
using BikeShopApp.Models;

namespace BikeShopApp.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(int orderId);
        Task<List<Order>> GetOrdersAsync();
        Task<bool> CreateOrderAsync(Order order, ICollection<int> productsIds);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<User?> GetUserOfOrderAsync(int orderId);
        Task<List<Order>> GetOrdersFromUserAsync(int userId);
        Task<OrdersResponseDto?> GetOrdersFromUserByPageAsync(int userId, string currentPage, string pageResults);
        Task<List<Product>> GetAllProductsInOrderAsync(int orderId);
        Task<List<Order>> GetAllOrdersFromAfterPeriodAsync(DateTime date);
        Task<bool> OrderExistsAsync(int orderId);
    }
}
