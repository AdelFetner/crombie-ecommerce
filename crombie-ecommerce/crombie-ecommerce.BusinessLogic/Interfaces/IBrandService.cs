using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;

namespace Interfaces
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(Guid id);
        Task<Brand> CreateBrand(BrandDto brandDto);
        Task<bool> ArchiveMethod(Guid categoryId, string processedBy = "Unregistered");
        Task<Brand> UpdateBrand(Guid id, Brand updatedBrand);
    }
}