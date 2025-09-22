using LaptopStoreApi.DTOs;

namespace LaptopStoreApi.Services
{
    public interface ILaptopService
    {
        Task<IEnumerable<LaptopDto>> GetAllLaptopsAsync();
        Task<LaptopDto?> GetLaptopByIdAsync(int id);
        Task<LaptopDto> CreateLaptopAsync(CreateLaptopDto createLaptopDto);
        Task<LaptopDto?> UpdateLaptopAsync(int id, UpdateLaptopDto updateLaptopDto);
        Task<bool> DeleteLaptopAsync(int id);
        Task<IEnumerable<LaptopDto>> SearchLaptopsAsync(string searchTerm);
        Task<IEnumerable<LaptopDto>> GetLaptopsByBrandAsync(string brand);
        Task<IEnumerable<LaptopDto>> GetLaptopsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}