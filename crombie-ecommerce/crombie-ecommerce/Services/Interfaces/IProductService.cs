using crombie_ecommerce.Models.Models.Entities;
using crombie_ecommerce.Models.Models.Dto;

namespace crombie_ecommerce.Services.Interfaces
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