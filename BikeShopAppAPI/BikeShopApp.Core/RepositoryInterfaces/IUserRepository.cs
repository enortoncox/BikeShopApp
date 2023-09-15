using BikeShopApp.Core.Models;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace BikeShopApp.Core.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(int userId);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<List<ApplicationUser>?> GetUsersAsync();
        Task<ApplicationUser?> CreateUserAsync(ApplicationUser applicationUser, string password);
        Task<bool> UpdateUserAsync(UserDto applicationUser);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> IsUserAdminAsync(int userId);
        Task<bool> ChangeAdminStatusAsync(int userId);
        Task<List<ApplicationUser>?> GetAllUsersThatStartWithLetterAsync(string letter);
        Task<bool> UserExistsAsync(int userId);
        Task<IdentityResult?> UpdateUserPasswordAsync(UserPasswordDto userPassword);
    }
}
