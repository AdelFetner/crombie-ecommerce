using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models.Entities

{
    [Table("Wishlist")]
    public class Wishlist
    {
        [Key]
        public Guid WishlistId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<Tag>? Tags { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<Notification>? Notifications { get; set; } = [];
    }
}