using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crombie_ecommerce.Models.Entities

{
    [Table("Wishlist")]
    public class Wishlist : IProcessableEntity
    {
        [Key]
        public Guid WishlistId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Product>? Products { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<Tag>? Tags { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<Notification>? Notifications { get; set; } = [];

        public Guid Id => WishlistId;
        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}