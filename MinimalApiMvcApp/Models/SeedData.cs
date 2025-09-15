using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MinimalApiMvcApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Look for any laptops.
                if (context.Laptops.Any())
                {
                    return;   // DB has been seeded
                }

                context.Laptops.AddRange(
                    new Laptop { Brand = "Dell", Model = "XPS 13", Price = 1200 },
                    new Laptop { Brand = "Apple", Model = "MacBook Pro", Price = 2000 },
                    new Laptop { Brand = "HP", Model = "Spectre x360", Price = 1500 }
                );
                context.SaveChanges();
            }
        }
    }
}
