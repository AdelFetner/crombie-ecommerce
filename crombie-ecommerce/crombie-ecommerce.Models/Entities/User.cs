using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MinLength(4)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public string Adress { get; set; }

        public bool IsVerified { get; set; }

        public virtual ICollection<Wishlist>? Wishlists { get; set; } = [];

        public virtual ICollection<Order>? Orders { get; set; } = [];
    }
}
