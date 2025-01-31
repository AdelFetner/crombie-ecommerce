using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopContext _context;

        public CategoryService(ShopContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategory(Guid id, Category updatedCategory)
        {
            // look up for the first category with the given id by reusing previous get by id method
            var existingCategory = await GetCategoryById(id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }

            // prop update
            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return existingCategory;

        }

        public async Task<bool> ArchiveMethod(Guid categoryId, string processedBy = "Unregistered")
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
                return false;

            var historyCategory = new HistoryCategory
            {
                OriginalId = category.CategoryId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = processedBy,
                EntityJson = category.SerializeToJson()
            };

            _context.HistoryCategories.Add(historyCategory);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}