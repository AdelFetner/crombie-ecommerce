using crombie_ecommerce.Models.Entities;
using crombie_ecommerce.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(ProductDto productDto, IFormFile fileImage);
        Task DeleteProduct(Guid id);
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetPage(int page, int quantity);
        Task<Product> GetProductById(Guid id);
        Task<Product> UpdateProduct(Guid id, ProductDto updatedProductDto);
        Task<List<Product>> FilterProductsAsync(ProductFilterDto filter);
    }
}