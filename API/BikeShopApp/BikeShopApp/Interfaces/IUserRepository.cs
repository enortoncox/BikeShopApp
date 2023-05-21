using BikeShopApp.Dto;
using BikeShopApp.Models;

namespace BikeShopApp.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetUsersAsync();
        Task<User?> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateUserPasswordAsync(UserPassword userPassword);
        Task<bool> IsUserAdminAsync(int userId);
        Task<bool> ChangeAdminStatusAsync(int userId);
        Task<List<User>> GetAllUsersThatStartWithLetterAsync(string letter);
        Task<bool> UserExistsAsync(int userId);
    }
}
