using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
{
    public class BrandService : IBrandService
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

        // Method to delete and archive brand
        public async Task<bool> ArchiveMethod(Guid brandId, string processedBy = "Unregistered")
        {
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandId == brandId);

            if (brand == null)
                return false;

            var historyBrand = new HistoryBrand
            {
                OriginalId = brand.BrandId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = processedBy,
                EntityJson = brand.SerializeToJson()
            };

            _context.HistoryBrands.Add(historyBrand);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}