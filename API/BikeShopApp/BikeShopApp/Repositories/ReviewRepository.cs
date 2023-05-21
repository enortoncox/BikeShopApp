using BikeShopApp.Data;
using BikeShopApp.Interfaces;
using BikeShopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShopApp.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await GetReviewAsync(reviewId);

            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<List<Review>> GetAllReviewsFromAUserAsync(int userId)
        {
            return _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
        }

        public Task<List<Review>> GetAllReviewsOfAProductAsync(int productId)
        {
            return _context.Reviews.Where(r => r.ProductId == productId).ToListAsync();
        }

        public Task<Review?> GetReviewAsync(int reviewId)
        {
            return _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public Task<bool> ReviewExistsAsync(int reviewId)
        {
            return _context.Reviews.AnyAsync(r => r.ReviewId == reviewId);
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
