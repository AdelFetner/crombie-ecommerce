using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models.Dto
{
    public class BrandDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Url]
        [MaxLength(255)]
        public string? WebsiteUrl { get; set; }
    }
}
