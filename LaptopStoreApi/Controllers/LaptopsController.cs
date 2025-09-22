using Microsoft.AspNetCore.Mvc;
using LaptopStoreApi.DTOs;
using LaptopStoreApi.Services;
using System.ComponentModel.DataAnnotations;

namespace LaptopStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LaptopsController : ControllerBase
    {
        private readonly ILaptopService _laptopService;
        private readonly ILogger<LaptopsController> _logger;

        public LaptopsController(ILaptopService laptopService, ILogger<LaptopsController> logger)
        {
            _laptopService = laptopService;
            _logger = logger;
        }

        /// <summary>
        /// Get all laptops
        /// </summary>
        /// <returns>List of all available laptops</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LaptopDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LaptopDto>>> GetAllLaptops()
        {
            try
            {
                var laptops = await _laptopService.GetAllLaptopsAsync();
                return Ok(laptops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all laptops");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get a laptop by ID
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>Laptop details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LaptopDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LaptopDto>> GetLaptop(int id)
        {
            try
            {
                var laptop = await _laptopService.GetLaptopByIdAsync(id);
                if (laptop == null)
                {
                    return NotFound($"Laptop with ID {id} not found");
                }
                return Ok(laptop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving laptop with ID {LaptopId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Create a new laptop
        /// </summary>
        /// <param name="createLaptopDto">Laptop creation data</param>
        /// <returns>Created laptop</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LaptopDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LaptopDto>> CreateLaptop([FromBody] CreateLaptopDto createLaptopDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var laptop = await _laptopService.CreateLaptopAsync(createLaptopDto);
                return CreatedAtAction(nameof(GetLaptop), new { id = laptop.Id }, laptop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating laptop");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update an existing laptop
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <param name="updateLaptopDto">Laptop update data</param>
        /// <returns>Updated laptop</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LaptopDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LaptopDto>> UpdateLaptop(int id, [FromBody] UpdateLaptopDto updateLaptopDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var laptop = await _laptopService.UpdateLaptopAsync(id, updateLaptopDto);
                if (laptop == null)
                {
                    return NotFound($"Laptop with ID {id} not found");
                }

                return Ok(laptop);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating laptop with ID {LaptopId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Delete a laptop
        /// </summary>
        /// <param name="id">Laptop ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLaptop(int id)
        {
            try
            {
                var result = await _laptopService.DeleteLaptopAsync(id);
                if (!result)
                {
                    return NotFound($"Laptop with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting laptop with ID {LaptopId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Search laptops by term
        /// </summary>
        /// <param name="searchTerm">Search term for brand, model, description, processor, or GPU</param>
        /// <returns>List of matching laptops</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<LaptopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LaptopDto>>> SearchLaptops([FromQuery] [Required] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }

                var laptops = await _laptopService.SearchLaptopsAsync(searchTerm);
                return Ok(laptops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching laptops with term {SearchTerm}", searchTerm);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get laptops by brand
        /// </summary>
        /// <param name="brand">Brand name</param>
        /// <returns>List of laptops from the specified brand</returns>
        [HttpGet("brand/{brand}")]
        [ProducesResponseType(typeof(IEnumerable<LaptopDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LaptopDto>>> GetLaptopsByBrand(string brand)
        {
            try
            {
                var laptops = await _laptopService.GetLaptopsByBrandAsync(brand);
                return Ok(laptops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving laptops for brand {Brand}", brand);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get laptops by price range
        /// </summary>
        /// <param name="minPrice">Minimum price</param>
        /// <param name="maxPrice">Maximum price</param>
        /// <returns>List of laptops within the price range</returns>
        [HttpGet("price-range")]
        [ProducesResponseType(typeof(IEnumerable<LaptopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LaptopDto>>> GetLaptopsByPriceRange(
            [FromQuery] [Range(0, double.MaxValue)] decimal minPrice,
            [FromQuery] [Range(0, double.MaxValue)] decimal maxPrice)
        {
            try
            {
                if (minPrice > maxPrice)
                {
                    return BadRequest("Minimum price cannot be greater than maximum price");
                }

                var laptops = await _laptopService.GetLaptopsByPriceRangeAsync(minPrice, maxPrice);
                return Ok(laptops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving laptops in price range {MinPrice}-{MaxPrice}", minPrice, maxPrice);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}