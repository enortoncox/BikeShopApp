using BikeShopApp.Data;
using BikeShopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShopApp
{
    public class AppDbInitialiser
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();


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
                    context.Users.Add(new User()
                    {
                        Name = "Admin",
                        Address = "1234 Fake Street",
                        Email = "admin@gmail.com",
                        ImgPath = "resources/images/user.jpg",
                        Password = "admin",
                        IsAdmin = true
                    });

                    context.SaveChanges();

                    var adminUser = context.Users.FirstOrDefault(u => u.Name == "Admin");

                    context.Carts.Add(new Cart()
                    {
                        TotalCost = 0,
                        TotalQuantity = 0,
                        UserId = adminUser.UserId
                    });

                    context.SaveChanges();

                    var adminCart = context.Carts.FirstOrDefault(c => c.UserId == adminUser.UserId);

                    adminUser.CartId = adminCart.CartId;

                    context.SaveChanges();


                    //User
                    context.Users.Add(new User()
                    {
                        Name = "User",
                        Address = "4567 Fake Street",
                        Email = "user@gmail.com",
                        ImgPath = "resources/images/user.jpg",
                        Password = "user",
                        IsAdmin = false
                    });

                    context.SaveChanges();

                    var user = context.Users.FirstOrDefault(u => u.Name == "User");

                    context.Carts.Add(new Cart()
                    {
                        TotalCost = 0,
                        TotalQuantity = 0,
                        UserId = user.UserId
                    });

                    context.SaveChanges();

                    var userCart = context.Carts.FirstOrDefault(c => c.UserId == user.UserId);

                    user.CartId = userCart.CartId;

                    context.SaveChanges();
                }

                //Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                    //Cat 1
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

                    //Cat 2
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

                    //Cat 3
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

                    //Cat 4
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

                context.SaveChanges();
            }
        }
    }
}
