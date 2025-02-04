using Microsoft.AspNetCore.Mvc;
using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("pages")]
        public async Task<ActionResult<IEnumerable<Product>>> GetPage([FromQuery] int page, [FromQuery] int pageSize)
        {
            var products = await _productService.GetPage(page, pageSize);
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductDto productDto, IFormFile fileImage)
        {
            try
            {
                var createdProduct = await _productService.CreateProduct(productDto, fileImage);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProduct(Guid id, ProductDto productDto)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(id, productDto);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAndArchive(Guid id)
        {
            var success = await _productService.ArchiveMethod(id, "Unregistered");
            if (!success)
            {
                return NotFound(new { message = "Product not found." });
            }
            return Ok(new { message = "Product deleted successfully." });
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<Product>>> FilterProducts(
            [FromQuery] ProductFilterDto filter)
        {
            var result = await _productService.FilterProductsAsync(filter);
            return Ok(result);
        }
    }
}
