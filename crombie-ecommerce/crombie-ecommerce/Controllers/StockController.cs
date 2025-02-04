using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace crombie_ecommerce.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddStock([FromBody] StockDto stockDto)
        {
            try
            {
                bool success = await _stockService.AddStockAsync(stockDto.ProductId, stockDto.Quantity);
                if (!success) return BadRequest("Stock record already exists for this product");
                return Ok("Stock added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateStock(Guid productId, [FromBody] int quantityChange)
        {
            try
            {
                bool success = await _stockService.UpdateStockAsync(productId, quantityChange);
                if (!success) return NotFound("Stock record not found");
                return Ok("Stock updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetStock(Guid productId)
        {
            var stock = await _stockService.GetStockByProductIdAsync(productId);
            if (stock == null) return NotFound("Stock record not found");
            return Ok(new { productId, stock });
        }
    }
}