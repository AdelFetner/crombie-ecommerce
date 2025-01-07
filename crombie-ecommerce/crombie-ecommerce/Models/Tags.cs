using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models
{
    [Table("Tags")]
    public class Tags
    {
        [Key]
        public Guid TagId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [JsonIgnore] 
        public Guid? WishlistId { get; set; }
        [JsonIgnore] 
        public virtual Wishlist? Wishlist { get; set; }
    }
}
