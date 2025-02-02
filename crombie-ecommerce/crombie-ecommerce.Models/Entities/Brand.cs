using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models.Entities
{
    public class Brand
    {
        [Key]
        [JsonIgnore]
        public Guid BrandId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Url]
        [MaxLength(255)]
        public string? WebsiteUrl { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public Guid Id => BrandId;
        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
