using BikeShopApp.Infrastructure.DatabaseContext;
using BikeShopApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using BikeShopApp.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BikeShopApp.Core.Enums;

namespace BikeShopApp.Infrastructure
{
    public class AppDbInitialiser
    {
        /// <summary>
        /// Seed example data into the database
        /// </summary>
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                //Categories
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(new Category()
                    {
                        Name = "Mountain Bikes" 
                    },
                    new Category()
                    {
                        Name = "Electric Bikes"
                    },
                    new Category()
                    {
                        Name = "Hybrid Bikes"
                    }, 
                    new Category()
                    {
                        Name = "Equipment"
                    });
                }

                //Users
                if (!context.Users.Any())
                {
                    //ADMIN
                    ApplicationUser adminInitial = new ApplicationUser()
                    {
                        Name = "Admin",
                        Address = "1234 Fake Street",
                        Email = "admin@example.com",
                        ImgPath = @"Resources\Images\user.jpg",
                        UserName = "admin@example.com"
                    };

                    var adminResult = await userManager.CreateAsync(adminInitial, "Adminpass1!");

                    if (!adminResult.Succeeded) 
                    {
                        var errorMessage = string.Join(" | ", adminResult.Errors.SelectMany(v => v.Description));
                        throw new Exception(errorMessage);
                    }

                    //Create User role if not already made
                    if (!await roleManager.RoleExistsAsync(RoleTypeOptions.User.ToString()))
                    {
                        ApplicationRole userRole = new ApplicationRole() { Name = RoleTypeOptions.User.ToString() };
                        await roleManager.CreateAsync(userRole);
                    }

                    var admin = await userManager.FindByEmailAsync(adminInitial.Email);

                    await userManager.AddToRoleAsync(admin, RoleTypeOptions.User.ToString());

                    //Create Admin role if not already made
                    if (!await roleManager.RoleExistsAsync(RoleTypeOptions.Admin.ToString()))
                    {
                        ApplicationRole adminRole = new ApplicationRole() { Name = RoleTypeOptions.Admin.ToString() };
                        await roleManager.CreateAsync(adminRole);
                    }

                    await userManager.AddToRoleAsync(admin, RoleTypeOptions.Admin.ToString());

                    context.Carts.Add(new Cart()
                    {
                    TotalCost = 0,
                    TotalQuantity = 0,
                    UserId = admin.Id
                    });


                    await context.SaveChangesAsync();

                    Cart adminCart = context.Carts.FirstOrDefault(c => c.UserId == admin.UserId);

                    admin.CartId = adminCart.CartId;

                    await context.SaveChangesAsync();

                    //User
                    var userInitial = new ApplicationUser()
                    {
                        Name = "User",
                        Address = "4567 Fake Street",
                        Email = "user@example.com",
                        ImgPath = @"Resources\Images\user.jpg",
                        UserName = "user@example.com"
                    };

                    var userResult = await userManager.CreateAsync(userInitial, "Userpass1!");

                    if (!userResult.Succeeded)
                    {
                        var errorMessage = string.Join(" | ", userResult.Errors.SelectMany(v => v.Description));
                        throw new Exception(errorMessage);
                    }

                    //Create User role if not already made
                    if (!await roleManager.RoleExistsAsync(RoleTypeOptions.User.ToString()))
                    {
                        ApplicationRole userRole = new ApplicationRole() { Name = RoleTypeOptions.User.ToString() };
                        await roleManager.CreateAsync(userRole);
                    }

                    var user = await userManager.FindByEmailAsync(userInitial.Email);

                    await userManager.AddToRoleAsync(user, RoleTypeOptions.User.ToString());

                    context.Carts.Add(new Cart()
                    {
                    TotalCost = 0,
                    TotalQuantity = 0,
                    UserId = user.UserId
                    });

                    await context.SaveChangesAsync();

                    Cart userCart = context.Carts.FirstOrDefault(c => c.UserId == user.UserId);

                    user.CartId = userCart.CartId;

                    await context.SaveChangesAsync();
                }

                //Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                    //Category 1
                    new Product()
                    {
                        Name = "Valour Mountain bike",
                        Price = 360,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 40,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Carbon Mountain Bike",
                        Price = 505,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 35,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Karkinos Mountain Bike",
                        Price = 350,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 30,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Vengence Mountain Bike",
                        Price = 455,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 20,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Disc Mountain Bike",
                        Price = 200,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 40,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Hellcat Mountain Bike",
                        Price = 250,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 45,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "ATB Mountain Bike",
                        Price = 800,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 50,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Slant Mountain Bike",
                        Price = 169,
                        ImgPath = "resources/images/mountain_bike.jpg",
                        Quantity = 40,
                        CategoryId = 1
                    },

                    //Category 2
                    new Product()
                    {
                        Name = "Vengeance Electric Bike",
                        Price = 360,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 40,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Boardman Electric Bike",
                        Price = 505,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 35,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Assist Electric Bike",
                        Price = 350,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 30,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "ADV Electric Bike",
                        Price = 455,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 20,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Rockmachine Electric Bike",
                        Price = 200,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 40,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Raleigh Electric Bike",
                        Price = 250,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 45,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "TRB Electric Bike",
                        Price = 800,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 50,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Eovolt Electric Bike",
                        Price = 169,
                        ImgPath = "resources/images/electric_bike.jpg",
                        Quantity = 40,
                        CategoryId = 2
                    },

                    //Category 3
                    new Product()
                    {
                        Name = "Crossfire Hybrid Bike",
                        Price = 360,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 40,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Parva Hybrid Bike",
                        Price = 505,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 35,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Array Hybrid Bike",
                        Price = 350,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 30,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Open Hybrid Bike",
                        Price = 455,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 20,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Frames Hybrid Bike",
                        Price = 200,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 40,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Raleigh Hybrid Bike",
                        Price = 250,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 45,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Carrera Hybrid Bike",
                        Price = 800,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 50,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Volt Hybrid Bike",
                        Price = 169,
                        ImgPath = "resources/images/hybrid_bike.jpg",
                        Quantity = 40,
                        CategoryId = 3
                    },

                    //Category 4
                    new Product()
                    {
                        Name = "Carna Bike Pump",
                        Price = 25,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 50,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Parva Bike Pump",
                        Price = 50,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 35,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Array Bike lock",
                        Price = 55,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 30,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Open Bike Lock",
                        Price = 45,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 20,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Frames Puncture kit",
                        Price = 35,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 40,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Raleigh Puncture kit",
                        Price = 45,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 45,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Carrera Bike Rack",
                        Price = 125,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 50,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Thule Bike Rack",
                        Price = 85,
                        ImgPath = "resources/images/bike_lock.jpg",
                        Quantity = 40,
                        CategoryId = 4
                    }
                    );
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
