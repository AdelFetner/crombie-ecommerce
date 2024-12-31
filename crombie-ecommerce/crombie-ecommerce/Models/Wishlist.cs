using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crombie_ecommerce.Models

{
    [Table("Wishlist")]
    public class Wishlist
    {
        [Key]
        public Guid WishlistId { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [MinLength(3)]
        public string Tag { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}