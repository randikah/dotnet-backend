using Microsoft.EntityFrameworkCore;
using LaptopStoreApi.Data;
using LaptopStoreApi.Models;

namespace LaptopStoreApi.Repositories
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly LaptopStoreDbContext _context;

        public LaptopRepository(LaptopStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Laptop>> GetAllAsync()
        {
            return await _context.Laptops
                .Where(l => l.IsAvailable)
                .OrderBy(l => l.Brand)
                .ThenBy(l => l.Model)
                .ToListAsync();
        }

        public async Task<Laptop?> GetByIdAsync(int id)
        {
            return await _context.Laptops
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Laptop> CreateAsync(Laptop laptop)
        {
            laptop.CreatedAt = DateTime.UtcNow;
            laptop.UpdatedAt = DateTime.UtcNow;

            _context.Laptops.Add(laptop);
            await _context.SaveChangesAsync();
            return laptop;
        }

        public async Task<Laptop?> UpdateAsync(int id, Laptop laptop)
        {
            var existingLaptop = await _context.Laptops.FindAsync(id);
            if (existingLaptop == null)
                return null;

            existingLaptop.Brand = laptop.Brand;
            existingLaptop.Model = laptop.Model;
            existingLaptop.Price = laptop.Price;
            existingLaptop.Processor = laptop.Processor;
            existingLaptop.RAM = laptop.RAM;
            existingLaptop.Storage = laptop.Storage;
            existingLaptop.GPU = laptop.GPU;
            existingLaptop.OperatingSystem = laptop.OperatingSystem;
            existingLaptop.ScreenSize = laptop.ScreenSize;
            existingLaptop.Description = laptop.Description;
            existingLaptop.IsAvailable = laptop.IsAvailable;
            existingLaptop.StockQuantity = laptop.StockQuantity;
            existingLaptop.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingLaptop;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
                return false;

            _context.Laptops.Remove(laptop);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Laptop>> SearchAsync(string searchTerm)
        {
            return await _context.Laptops
                .Where(l => l.IsAvailable && 
                           (l.Brand.Contains(searchTerm) || 
                            l.Model.Contains(searchTerm) || 
                            l.Description.Contains(searchTerm) ||
                            l.Processor.Contains(searchTerm) ||
                            l.GPU.Contains(searchTerm)))
                .OrderBy(l => l.Brand)
                .ThenBy(l => l.Model)
                .ToListAsync();
        }

        public async Task<IEnumerable<Laptop>> GetByBrandAsync(string brand)
        {
            return await _context.Laptops
                .Where(l => l.IsAvailable && l.Brand.ToLower() == brand.ToLower())
                .OrderBy(l => l.Model)
                .ToListAsync();
        }

        public async Task<IEnumerable<Laptop>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Laptops
                .Where(l => l.IsAvailable && l.Price >= minPrice && l.Price <= maxPrice)
                .OrderBy(l => l.Price)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Laptops.AnyAsync(l => l.Id == id);
        }
    }
}