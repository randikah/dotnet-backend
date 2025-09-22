using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Models
{
    public class Laptop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [StringLength(50)]
        public string Processor { get; set; } = string.Empty;

        [Range(1, 128)]
        public int RAM { get; set; } // In GB

        [Range(1, 8192)]
        public int Storage { get; set; } // In GB

        [StringLength(50)]
        public string GPU { get; set; } = string.Empty;

        [StringLength(20)]
        public string OperatingSystem { get; set; } = string.Empty;

        [Range(10, 20)]
        public double ScreenSize { get; set; } // In inches

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsAvailable { get; set; } = true;

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}