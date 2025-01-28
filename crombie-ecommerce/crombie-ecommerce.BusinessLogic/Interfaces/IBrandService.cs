using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;

namespace Interfaces
{
    public interface IBrandService
    {
        Task<Brand> CreateBrand(BrandDto brandDto);
        Task DeleteBrand(Guid id);
        Task<List<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(Guid id);
        Task<Brand> UpdateBrand(Guid id, Brand updatedBrand);
    }
}