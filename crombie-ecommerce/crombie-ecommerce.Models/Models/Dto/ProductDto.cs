using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models.Dto
{
    public class ProductDto
    {
        [Required]
        [MinLength(4)]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
    }
}
