using crombie_ecommerce.Models.Entities;

namespace crombie_ecommerce.Models.Entities
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
