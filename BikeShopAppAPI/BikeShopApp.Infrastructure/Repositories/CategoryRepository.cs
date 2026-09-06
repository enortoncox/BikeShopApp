using BikeShopApp.Core.Models;
using BikeShopApp.Infrastructure.DatabaseContext;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeShopApp.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> CategoryExistsAsync(int categoryId)
        {
            return _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await GetCategoryAsync(categoryId);

            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            return _context.Categories.ToListAsync();
        }

        public Task<List<Product>> GetAllProductsByCategoryAsync(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        }

        public Task<Category?> GetCategoryAsync(int categoryId)
        {
            return _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
