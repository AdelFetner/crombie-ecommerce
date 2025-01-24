using Amazon.S3.Model;
using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace crombie_ecommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopContext _context;
        private readonly s3Service _s3Service;
        private readonly string _bucketFolder = "product";

        public ProductService(ShopContext context, s3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products
                .Include(p => p.Categories)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.ProductId == id); // Couldn't make previous FindAsync work 
        }

        public async Task<Product> CreateProduct(ProductDto productDto, IFormFile fileImage)
        {
            // gets categories from db. Todo: improve this, only way I thought of making the categories work without declaring other attributes aside from id
            var categories = await _context.Categories
                .Where(c => productDto.CategoryIds.Contains(c.CategoryId))
                .ToListAsync();

            using var stream = fileImage.OpenReadStream();

            string bucketFolder = "product";

            var upload = await _s3Service.UploadFileAsync(stream, fileImage.FileName, fileImage.ContentType, bucketFolder);

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                BrandId = productDto.BrandId,
                Categories = categories,
                Image = $"{bucketFolder}/{fileImage.FileName}"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Guid id, ProductDto updatedProductDto)
        {
            // look up for the first product with the given id by reusing previous get by id method
            var existingProduct = await GetProductById(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            var categories = await _context.Categories
                .Where(c => updatedProductDto.CategoryIds.Contains(c.CategoryId))
                .ToListAsync();

            // prop update
            existingProduct.Name = updatedProductDto.Name;
            existingProduct.Description = updatedProductDto.Description;
            existingProduct.Price = updatedProductDto.Price;
            existingProduct.Categories = categories;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return existingProduct;
        }

        public async Task DeleteProduct(Guid id)
        {
            var existingProduct = await GetProductById(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            var upload = await _s3Service.DeleteObjectFromBucketAsync(existingProduct.Image);

            _context.Products.Remove(existingProduct);
            await _context.SaveChangesAsync();
        }

        //pagination logic
        public async Task<List<Product>> GetPage(int page, int quantity)
        {
            return await _context.Products
                .Skip((page - 1) * quantity)
                .Take(quantity)
                .Include(p => p.Categories)
                .ToListAsync();
        }
    }
}