using crombie_ecommerce.Models.Entities;

namespace crombie_ecommerce.Models.Dto
{
    public class CartDto
    {
        public Guid CartId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }

        public Guid UserId { get; set; }
    }
}
