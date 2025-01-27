namespace crombie_ecommerce.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount => Items.Sum(item => item.Total);

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
