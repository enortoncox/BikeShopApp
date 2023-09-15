using BikeShopApp.Core.Models;
using BikeShopApp.Infrastructure.DatabaseContext;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using BikeShopApp.Core.Identity;
using Microsoft.AspNetCore.Identity;
using BikeShopApp.Core.Enums;

namespace BikeShopApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> ChangeAdminStatusAsync(int userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) 
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(user, RoleTypeOptions.Admin.ToString()))
            {
                await _userManager.RemoveFromRoleAsync(user, RoleTypeOptions.Admin.ToString());
            }
            else 
            {
                if (!await _roleManager.RoleExistsAsync(RoleTypeOptions.Admin.ToString())) 
                {
                    ApplicationRole adminRole = new ApplicationRole() { Name = RoleTypeOptions.Admin.ToString() };
                    await _roleManager.CreateAsync(adminRole);
                }

                await _userManager.AddToRoleAsync(user, RoleTypeOptions.Admin.ToString());
            }

            return true;
        }

        public async Task<ApplicationUser?> CreateUserAsync(ApplicationUser applicationUser, string password)
        {
            var result  = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                Cart cart = new Cart
                {
                    TotalCost = 0,
                    TotalQuantity = 0,
                    UserId = applicationUser.UserId
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                applicationUser.CartId = cart.CartId;

                await _context.SaveChangesAsync();

                return applicationUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                //Delete Cart
                Cart? cart = await _context.Carts.FindAsync(user.CartId);

                if (cart != null) 
                {
                    _context.Carts.Remove(cart);
                    await _context.SaveChangesAsync();
                }

                //Delete User
                await _userManager.DeleteAsync(user);
                return true;
            }

            return false;
        }

        public Task<List<ApplicationUser>?> GetAllUsersThatStartWithLetterAsync(string letter)
        {
            return _context.Users.Where(u => u.Name.StartsWith(letter)).ToListAsync();
        }

        public Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<ApplicationUser?> GetUserByIdAsync(int userId)
        {
            return _userManager.FindByIdAsync(userId.ToString());
        }

        public Task<List<ApplicationUser>?> GetUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        public async Task<bool> IsUserAdminAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            var result = false;

            if (user != null) 
            {
                if (!await _roleManager.RoleExistsAsync(RoleTypeOptions.Admin.ToString()))
                {
                    ApplicationRole adminRole = new ApplicationRole() { Name = RoleTypeOptions.Admin.ToString() };
                    await _roleManager.CreateAsync(adminRole);
                }

                result = await _userManager.IsInRoleAsync(user, RoleTypeOptions.Admin.ToString());
            }

            return result;
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            var applicationUser = await _userManager.FindByEmailAsync(userDto.Email);

            if (applicationUser != null)
            {
                applicationUser.Email = userDto.Email;
                applicationUser.Name = userDto.Name;
                applicationUser.Address = userDto.Address;
                applicationUser.ImgPath = userDto.ImgPath;

                var result = await _userManager.UpdateAsync(applicationUser);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }

        public async Task<IdentityResult?> UpdateUserPasswordAsync(UserPasswordDto userPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userPassword.UserId);

            if (user == null)
            {
                return null;
            }

            return await _userManager.ChangePasswordAsync(user, userPassword.OldPassword, userPassword.NewPassword);
        }

        public Task<bool> UserExistsAsync(int userId)
        {
            return _context.Users.AnyAsync(user => user.Id == userId);
        }
    }
}
