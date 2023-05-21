using BikeShopApp.Models;

namespace BikeShopApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetReviewAsync(int reviewId);
        Task<bool> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<List<Review>> GetAllReviewsFromAUserAsync(int userId);
        Task<List<Review>> GetAllReviewsOfAProductAsync(int productId);
        Task<bool> ReviewExistsAsync(int reviewId);
    }
}
