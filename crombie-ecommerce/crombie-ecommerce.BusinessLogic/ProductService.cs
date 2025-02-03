using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
{
    public class ProductService : IProductService
    {
        private readonly ShopContext _context;
        private readonly s3Service _s3Service;
        private readonly string _bucketFolder = "products";

        public ProductService(ShopContext context, s3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Stock) // added stock to the include
                .ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Stock)
                .FirstOrDefaultAsync(p => p.ProductId == id); // Couldn't make previous FindAsync work 
        }

        public async Task<Product> CreateProduct(ProductDto productDto, IFormFile fileImage)
        {
            // gets categories from db. Todo: improve this, only way I thought of making the categories work without declaring other attributes aside from id
            var categories = await _context.Categories
                .Where(c => productDto.CategoryIds.Contains(c.CategoryId))
                .ToListAsync();

            var productId = Guid.NewGuid();

            var product = new Product
            {
                ProductId = productId,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                BrandId = productDto.BrandId,
                Categories = categories,
                Image = $"{_bucketFolder}/{productId}/{fileImage.FileName}",
                Stock = new Stock
                {
                    StockId = Guid.NewGuid(),
                    ProductId = productId,
                    Quantity = 0, 
                    LastUpdated = DateTime.UtcNow
                }
            };

            using var stream = fileImage.OpenReadStream();

            var upload = await _s3Service.UploadFileAsync(stream, fileImage.FileName, fileImage.ContentType, $"{_bucketFolder}/{product.ProductId}");

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

            // request to s3 to delete the object in the s3 bucket
            var request = await _s3Service.DeleteObjectFromBucketAsync(existingProduct.Image);

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
                .Include(p => p.Stock)
                .ToListAsync();
        }

        public async Task<List<Product>> FilterProductsAsync(ProductFilterDto filter)
        {

            // prepares the variable with the load of things to filter from
            var query = _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Brand)
            .Include(p => p.Wishlists)
            .Include(p => p.Stock)
            .AsQueryable();

            // price filtering (min max)
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            // brand filtering
            if (filter.BrandIds?.Any() == true)
            {
                query = query.Where(p => filter.BrandIds.Contains(p.BrandId));
            }
            
            // category filtering
            if (filter.CategoryIds?.Any() == true)
            {
                query = query.Where(p => p.Categories.Any(c => filter.CategoryIds.Contains(c.CategoryId)));
            }

            // wishlist filtering (placeholder)
            if (filter.UserId.HasValue)
            {
                query = query.Where(p => p.Wishlists.Any(w => w.UserId == filter.UserId));
            }

            // search term filtering (find keyword)
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(filter.SearchTerm) ||
                    p.Description.Contains(filter.SearchTerm));
            }

            // stock filtering
            if (filter.InStock.HasValue && filter.InStock.Value)
            {
                query = query.Where(p => p.Stock.Quantity > 0);
            }


            return await query.ToListAsync();
        }

        public async Task<bool> ArchiveMethod(Guid ProductId, string processedBy = "Unregistered")
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Brand)
                .Include(p => p.Image)
                .FirstOrDefaultAsync(p => p.ProductId == ProductId);

            if (product == null)
                return false;

            var historyProduct = new HistoryProduct
            {
                OriginalId = product.ProductId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = processedBy,
                EntityJson = product.SerializeToJson()
            };

            _context.HistoryProducts.Add(historyProduct);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}