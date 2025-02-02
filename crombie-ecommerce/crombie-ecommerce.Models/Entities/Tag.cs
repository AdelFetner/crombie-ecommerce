using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace crombie_ecommerce.Models.Entities
{
    [Table("Tag")]
    public class Tag
    {
        [Key]
        public Guid TagId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public Guid? WishlistId { get; set; }

        [JsonIgnore]
        public virtual Wishlist? Wishlist { get; set; }
        public Guid Id => TagId;
        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
