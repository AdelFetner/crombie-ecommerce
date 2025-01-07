using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Services
{
    public class ProductService
    {
        private readonly ShopContext _context;

        public ProductService(ShopContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Guid id, Product updatedProduct)
        {
            // look up for the first product with the given id by reusing previous get by id method
            var existingProduct = await GetProductById(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            // prop update
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;

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

            _context.Products.Remove(existingProduct);
            await _context.SaveChangesAsync();
        }
    }
}