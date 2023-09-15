using BikeShopApp.Core.Models;
using BikeShopApp.Core.DTO;

namespace BikeShopApp.Core.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductAsync(int productId);
        Task<List<Product>> GetProductsAsync(string category);
        Task<ProductsPageResponseDto?> GetProductsByPageAsync(string category, string currentPage, string pageResults);
        Task<ProductsPageResponseDto?> GetFilteredProductsByPageAsync(string category, string currentPage, string pageResults, string price, string rating);
        Task<bool> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<List<Product>> GetAllProductsWithQuantityOrLowerAsync(int quantity);
        Task<List<Product>> GetAllProductsThatStartWithLetterAsync(string letter);
        Task<bool> DecreaseQuantityAsync(int productId);
        Task<bool> SetAvgRatingOfProductAsync(int productId);
        Task<bool> ProductExistsAsync(int productId);
    }
}
