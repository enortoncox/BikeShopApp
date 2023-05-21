using BikeShopApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace BikeShopApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrdersProducts> OrdersProducts { get; set; }
        public DbSet<CartsProducts> CartsProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            //modelBuilder.Entity<Product>().HasCheckConstraint("CK_Product_Price", "[Price] >= 0");
            modelBuilder.Entity<Product>().ToTable(t => t.HasCheckConstraint("CK_Product_Price", "[Price] >= 0"));

            modelBuilder.Entity<CartsProducts>().HasKey(cp => new { cp.CartId, cp.ProductId });
            modelBuilder.Entity<OrdersProducts>().HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(19,4)");
            modelBuilder.Entity<Order>().Property(o => o.TotalPrice).HasColumnType("decimal(19,4)");
            modelBuilder.Entity<Cart>().Property(c => c.TotalCost).HasColumnType("decimal(19,4)");

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
