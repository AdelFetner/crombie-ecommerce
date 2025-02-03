using Microsoft.AspNetCore.Mvc;
using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using crombie_ecommerce.Models.Dto;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _categoryService.GetAllCategories();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Category>> PostCategory(CategoryDto categoryDto)
        {
            try
            {
                var createdCategory = await _categoryService.CreateCategory(categoryDto);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, createdCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCategory(Guid id, Category category)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategory(id, category);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAndArchive(Guid id)
        {
            var success = await _categoryService.ArchiveMethod(id, "Unregistered");
            if (!success)
            {
                return NotFound(new { message = "Category not found." });
            }
            return Ok(new { message = "Category deleted successfully." });
        }
    }
}
