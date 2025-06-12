using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OnlineStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Category);
            });

            // CartItem configuration
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.SessionId);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasIndex(e => e.CustomerEmail);
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Лаптоп Lenovo", Description = "15.6\" FHD, Intel i5, 8GB RAM", Price = 1299.99m, StockQuantity = 10, Category = "Електроника", ImageUrl = "/images/laptop1.jpg" },
                new Product { Id = 2, Name = "Смартфон Samsung", Description = "6.1\" AMOLED, 128GB", Price = 699.99m, StockQuantity = 25, Category = "Електроника", ImageUrl = "/images/phone1.jpg" },
                new Product { Id = 3, Name = "Безжични слушалки", Description = "Bluetooth 5.0, активно потискане на шума", Price = 199.99m, StockQuantity = 50, Category = "Аксесоари", ImageUrl = "/images/headphones1.jpg" },
                new Product { Id = 4, Name = "Геймърска мишка", Description = "RGB подсветка, 12000 DPI", Price = 89.99m, StockQuantity = 30, Category = "Аксесоари", ImageUrl = "/images/mouse1.jpg" },
                new Product { Id = 5, Name = "Механична клавиатура", Description = "Cherry MX Red суичове", Price = 149.99m, StockQuantity = 20, Category = "Аксесоари", ImageUrl = "/images/keyboard1.jpg" }
            );
        }
    }
}