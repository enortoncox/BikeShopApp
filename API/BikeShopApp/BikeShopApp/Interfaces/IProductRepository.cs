using BikeShopApp.Dto;
using BikeShopApp.Models;

namespace BikeShopApp.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductAsync(int productId);
        Task<List<Product>> GetProductsAsync(string category);
        Task<ProductsResponseDto?> GetProductsByPageAsync(string category, string currentPage, string pageResults);
        Task<ProductsResponseDto?> GetFilteredProductsByPageAsync(string category, string currentPage, string pageResults, string price, string rating);
        Task<bool> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<List<Product>> GetAllProductsWithQuantityOrLowerAsync(int quantity);
        Task<List<Product>> GetAllProductsThatStartWithLetterAsync(string letter);
        Task<bool> DecreaseQuantityAsync(int productId);
        Task SetAvgRatingOfProductAsync(int productId);
        Task<bool> ProductExistsAsync(int productId);




    }
}
