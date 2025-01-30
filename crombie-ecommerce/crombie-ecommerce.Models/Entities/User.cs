using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models.Entities
{
    [Table("User")]
    public class User : IProcessableEntity
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

        public string Address { get; set; }

        public bool IsVerified { get; set; }

        public string? Image { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } 

        public virtual ICollection<Wishlist>? Wishlists { get; set; } = [];

        public virtual ICollection<Order>? Orders { get; set; } = [];
        public Guid Id => UserId;
        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
