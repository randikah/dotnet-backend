using Microsoft.EntityFrameworkCore;
using LaptopStoreApi.Models;

namespace LaptopStoreApi.Data
{
    public class LaptopStoreDbContext : DbContext
    {
        public LaptopStoreDbContext(DbContextOptions<LaptopStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Laptop> Laptops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Laptop entity
            modelBuilder.Entity<Laptop>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Processor)
                    .HasMaxLength(50);

                entity.Property(e => e.GPU)
                    .HasMaxLength(50);

                entity.Property(e => e.OperatingSystem)
                    .HasMaxLength(20);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("UTC_TIMESTAMP()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("UTC_TIMESTAMP()");

                entity.Property(e => e.IsAvailable)
                    .HasDefaultValue(true);

                // Create index for common search fields
                entity.HasIndex(e => e.Brand);
                entity.HasIndex(e => e.Model);
                entity.HasIndex(e => e.Price);
                entity.HasIndex(e => e.IsAvailable);
            });

            // Seed data
            modelBuilder.Entity<Laptop>().HasData(
                new Laptop
                {
                    Id = 1,
                    Brand = "Dell",
                    Model = "XPS 13",
                    Price = 999.99m,
                    Processor = "Intel Core i7-1165G7",
                    RAM = 16,
                    Storage = 512,
                    GPU = "Intel Iris Xe Graphics",
                    OperatingSystem = "Windows 11",
                    ScreenSize = 13.3,
                    Description = "Premium ultrabook with excellent performance and portability",
                    StockQuantity = 15,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Laptop
                {
                    Id = 2,
                    Brand = "MacBook",
                    Model = "Air M2",
                    Price = 1199.99m,
                    Processor = "Apple M2",
                    RAM = 8,
                    Storage = 256,
                    GPU = "Apple M2 GPU",
                    OperatingSystem = "macOS",
                    ScreenSize = 13.6,
                    Description = "Lightweight laptop with Apple's latest M2 chip",
                    StockQuantity = 10,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Laptop
                {
                    Id = 3,
                    Brand = "ASUS",
                    Model = "ROG Strix G15",
                    Price = 1499.99m,
                    Processor = "AMD Ryzen 7 5800H",
                    RAM = 16,
                    Storage = 1024,
                    GPU = "NVIDIA RTX 3060",
                    OperatingSystem = "Windows 11",
                    ScreenSize = 15.6,
                    Description = "High-performance gaming laptop",
                    StockQuantity = 8,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}