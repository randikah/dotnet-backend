using LaptopStoreApi.Models;

namespace LaptopStoreApi.Repositories
{
    public interface ILaptopRepository
    {
        Task<IEnumerable<Laptop>> GetAllAsync();
        Task<Laptop?> GetByIdAsync(int id);
        Task<Laptop> CreateAsync(Laptop laptop);
        Task<Laptop?> UpdateAsync(int id, Laptop laptop);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Laptop>> SearchAsync(string searchTerm);
        Task<IEnumerable<Laptop>> GetByBrandAsync(string brand);
        Task<IEnumerable<Laptop>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<bool> ExistsAsync(int id);
    }
}