using crombie_ecommerce.Models;

namespace crombie_ecommerce.Services.Interfaces
{
    public interface IBrandService
    {
        Task<Brand> CreateBrand(Brand brand);
        Task DeleteBrand(Guid id);
        Task<List<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(Guid id);
        Task<Brand> UpdateBrand(Guid id, Brand updatedBrand);
    }
}