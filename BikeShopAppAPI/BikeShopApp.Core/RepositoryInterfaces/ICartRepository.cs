using BikeShopApp.Core.Models;

namespace BikeShopApp.Core.RepositoryInterfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(int cartId);
        Task<bool> CreateCartAsync(int userId);
        Task<bool> AddToCartAsync(int cartId, int productId);
        Task<bool> RemoveProductFromCartAsync(int cartId, int productId);
        Task<List<Product>> GetProductsInCartAsync(int cartId);
        Task<int> GetNumberOfItemsInCartAsync(int cartId);
        Task<decimal> GetTotalPriceOfCartAsync(int cartId);
        Task<bool> EmptyCartAsync(int cartId);
        Task<bool> CartExistsAsync(int cartId);
    }
}
