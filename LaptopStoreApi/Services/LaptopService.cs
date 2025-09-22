using LaptopStoreApi.DTOs;
using LaptopStoreApi.Models;
using LaptopStoreApi.Repositories;

namespace LaptopStoreApi.Services
{
    public class LaptopService : ILaptopService
    {
        private readonly ILaptopRepository _laptopRepository;

        public LaptopService(ILaptopRepository laptopRepository)
        {
            _laptopRepository = laptopRepository;
        }

        public async Task<IEnumerable<LaptopDto>> GetAllLaptopsAsync()
        {
            var laptops = await _laptopRepository.GetAllAsync();
            return laptops.Select(MapToDto);
        }

        public async Task<LaptopDto?> GetLaptopByIdAsync(int id)
        {
            var laptop = await _laptopRepository.GetByIdAsync(id);
            return laptop != null ? MapToDto(laptop) : null;
        }

        public async Task<LaptopDto> CreateLaptopAsync(CreateLaptopDto createLaptopDto)
        {
            var laptop = new Laptop
            {
                Brand = createLaptopDto.Brand,
                Model = createLaptopDto.Model,
                Price = createLaptopDto.Price,
                Processor = createLaptopDto.Processor,
                RAM = createLaptopDto.RAM,
                Storage = createLaptopDto.Storage,
                GPU = createLaptopDto.GPU,
                OperatingSystem = createLaptopDto.OperatingSystem,
                ScreenSize = createLaptopDto.ScreenSize,
                Description = createLaptopDto.Description,
                StockQuantity = createLaptopDto.StockQuantity,
                IsAvailable = true
            };

            var createdLaptop = await _laptopRepository.CreateAsync(laptop);
            return MapToDto(createdLaptop);
        }

        public async Task<LaptopDto?> UpdateLaptopAsync(int id, UpdateLaptopDto updateLaptopDto)
        {
            var existingLaptop = await _laptopRepository.GetByIdAsync(id);
            if (existingLaptop == null)
                return null;

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(updateLaptopDto.Brand))
                existingLaptop.Brand = updateLaptopDto.Brand;
            
            if (!string.IsNullOrWhiteSpace(updateLaptopDto.Model))
                existingLaptop.Model = updateLaptopDto.Model;
            
            if (updateLaptopDto.Price.HasValue)
                existingLaptop.Price = updateLaptopDto.Price.Value;
            
            if (!string.IsNullOrWhiteSpace(updateLaptopDto.Processor))
                existingLaptop.Processor = updateLaptopDto.Processor;
            
            if (updateLaptopDto.RAM.HasValue)
                existingLaptop.RAM = updateLaptopDto.RAM.Value;
            
            if (updateLaptopDto.Storage.HasValue)
                existingLaptop.Storage = updateLaptopDto.Storage.Value;
            
            if (!string.IsNullOrWhiteSpace(updateLaptopDto.GPU))
                existingLaptop.GPU = updateLaptopDto.GPU;
            
            if (!string.IsNullOrWhiteSpace(updateLaptopDto.OperatingSystem))
                existingLaptop.OperatingSystem = updateLaptopDto.OperatingSystem;
            
            if (updateLaptopDto.ScreenSize.HasValue)
                existingLaptop.ScreenSize = updateLaptopDto.ScreenSize.Value;
            
            if (!string.IsNullOrWhiteSpace(updateLaptopDto.Description))
                existingLaptop.Description = updateLaptopDto.Description;
            
            if (updateLaptopDto.StockQuantity.HasValue)
                existingLaptop.StockQuantity = updateLaptopDto.StockQuantity.Value;
            
            if (updateLaptopDto.IsAvailable.HasValue)
                existingLaptop.IsAvailable = updateLaptopDto.IsAvailable.Value;

            var updatedLaptop = await _laptopRepository.UpdateAsync(id, existingLaptop);
            return updatedLaptop != null ? MapToDto(updatedLaptop) : null;
        }

        public async Task<bool> DeleteLaptopAsync(int id)
        {
            return await _laptopRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<LaptopDto>> SearchLaptopsAsync(string searchTerm)
        {
            var laptops = await _laptopRepository.SearchAsync(searchTerm);
            return laptops.Select(MapToDto);
        }

        public async Task<IEnumerable<LaptopDto>> GetLaptopsByBrandAsync(string brand)
        {
            var laptops = await _laptopRepository.GetByBrandAsync(brand);
            return laptops.Select(MapToDto);
        }

        public async Task<IEnumerable<LaptopDto>> GetLaptopsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var laptops = await _laptopRepository.GetByPriceRangeAsync(minPrice, maxPrice);
            return laptops.Select(MapToDto);
        }

        private static LaptopDto MapToDto(Laptop laptop)
        {
            return new LaptopDto
            {
                Id = laptop.Id,
                Brand = laptop.Brand,
                Model = laptop.Model,
                Price = laptop.Price,
                Processor = laptop.Processor,
                RAM = laptop.RAM,
                Storage = laptop.Storage,
                GPU = laptop.GPU,
                OperatingSystem = laptop.OperatingSystem,
                ScreenSize = laptop.ScreenSize,
                Description = laptop.Description,
                CreatedAt = laptop.CreatedAt,
                UpdatedAt = laptop.UpdatedAt,
                IsAvailable = laptop.IsAvailable,
                StockQuantity = laptop.StockQuantity
            };
        }
    }
}