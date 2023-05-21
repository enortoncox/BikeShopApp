using BikeShopApp.Data;
using BikeShopApp.Dto;
using BikeShopApp.Interfaces;
using BikeShopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BikeShopApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeAdminStatusAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user != null) 
            {
                user.IsAdmin = !user.IsAdmin;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            _context.Users.Add(user);

            if (await _context.SaveChangesAsync() > 0)
            {
                Cart cart = new Cart
                {
                    TotalCost = 0,
                    TotalQuantity = 0,
                    UserId = user.UserId
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                user.CartId = cart.CartId;

                await _context.SaveChangesAsync();

                return user;
            }
            else 
            {
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user != null) 
            {
                _context.Users.Remove(user);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<List<User>> GetAllUsersThatStartWithLetterAsync(string letter)
        {
            return _context.Users.Where(u => u.Name.StartsWith(letter)).ToListAsync();
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User?> GetUserByIdAsync(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        public async Task<bool> IsUserAdminAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user != null && user.IsAdmin)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            var originalUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.UserId == updatedUser.UserId);

            if (originalUser != null) 
            {
                updatedUser.Password = originalUser.Password;
                _context.Users.Update(updatedUser);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserPasswordAsync(UserPassword userPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == userPassword.UserId);

            if (user != null) 
            {
                user.Password = userPassword.Password;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> UserExistsAsync(int userId)
        {
            return _context.Users.AnyAsync(user => user.UserId == userId);
        }
    }
}
