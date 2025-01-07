using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Services
{
    public class BrandService
    {
        private readonly ShopContext _context;

        public BrandService(ShopContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandById(Guid id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task<Brand> CreateBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand> UpdateBrand(Guid id, Brand updatedBrand)
        {
            var existingBrand = await GetBrandById(id);
            if (existingBrand == null)
            {
                throw new Exception("Brand not found");
            }

            existingBrand.Name = updatedBrand.Name;
            existingBrand.Description = updatedBrand.Description;
            existingBrand.WebsiteUrl = updatedBrand.WebsiteUrl;

            _context.Brands.Update(existingBrand);
            await _context.SaveChangesAsync();

            return existingBrand;
        }

        public async Task DeleteBrand(Guid id)
        {
            var brand = await GetBrandById(id);
            if (brand == null)
            {
                throw new Exception("Brand not found");
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }
    }
}