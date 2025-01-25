using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models.Models.Dto
{
    public class NotificationDTO
    {
        public string NotificationType { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid WishlistId { get; set; }
    }
}
