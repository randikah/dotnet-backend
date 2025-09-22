namespace LaptopStoreApi.DTOs
{
    public class LaptopDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Processor { get; set; } = string.Empty;
        public int RAM { get; set; } // In GB
        public int Storage { get; set; } // In GB
        public string GPU { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public double ScreenSize { get; set; } // In inches
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAvailable { get; set; }
        public int StockQuantity { get; set; }
    }
}