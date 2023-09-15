using BikeShopApp.Core.Models;

namespace BikeShopApp.Core.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryAsync(int categoryId);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<List<Product>> GetAllProductsByCategoryAsync(int categoryId);
        Task<List<Category>> GetCategoriesAsync();
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
