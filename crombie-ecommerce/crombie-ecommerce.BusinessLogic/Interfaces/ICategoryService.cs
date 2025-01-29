using crombie_ecommerce.Models.Entities;

namespace Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(Category category);
        Task<bool> ArchiveMethod(Guid categoryId, string processedBy = "Unregistered");
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(Guid id);
        Task<Category> UpdateCategory(Guid id, Category updatedCategory);
    }
}