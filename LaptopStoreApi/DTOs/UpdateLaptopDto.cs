using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.DTOs
{
    public class UpdateLaptopDto
    {
        [StringLength(100)]
        public string? Brand { get; set; }

        [StringLength(100)]
        public string? Model { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal? Price { get; set; }

        [StringLength(50)]
        public string? Processor { get; set; }

        [Range(1, 128)]
        public int? RAM { get; set; } // In GB

        [Range(1, 8192)]
        public int? Storage { get; set; } // In GB

        [StringLength(50)]
        public string? GPU { get; set; }

        [StringLength(20)]
        public string? OperatingSystem { get; set; }

        [Range(10, 20)]
        public double? ScreenSize { get; set; } // In inches

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        public bool? IsAvailable { get; set; }
    }
}