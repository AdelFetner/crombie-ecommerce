using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

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

        [Required]
        public string Image { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = [];

        public virtual ICollection<Wishlist>? Wishlists { get; set; } = [];

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<Notification>? Notifications { get; set; } = [];
        public Guid Id => ProductId;
        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
