using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models
{
    public class Product
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; } 

        [Required]
        [MinLength(4)]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [JsonIgnore]
        public Guid BrandId { get; set; }

        [JsonIgnore]
        public virtual Brand Brand { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [JsonIgnore]
        public Guid? UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }

        [JsonIgnore]
        public Guid? WishlistId { get; set; }
        [JsonIgnore]
        public virtual Wishlist? Wishlist { get; set; }


        //create a new guid each time this is initialized
        public Product()
        {
            Id = Guid.NewGuid(); // Generate a new GUID for the Id
        }

    }
}
