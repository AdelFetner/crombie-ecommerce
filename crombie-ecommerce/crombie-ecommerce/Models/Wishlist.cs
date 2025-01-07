using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models

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
        [JsonIgnore]
        public Guid? UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }

        [JsonIgnore]
        public Guid? ProductId { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }

        [JsonIgnore]
        public Guid? TagsId { get; set; }
        [JsonIgnore]
        public virtual ICollection<Tags>? Tags { get; set; }
    }
}