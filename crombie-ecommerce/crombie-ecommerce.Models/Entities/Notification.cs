using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models.Entities
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string NotificationType { get; set; }
        [Required]
        [MaxLength(100)]
        public string Message { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; } = false;

        [JsonIgnore]
        public Guid WishlistId { get; set; }
        public virtual Wishlist Wishlist { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}