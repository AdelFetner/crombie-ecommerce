using crombie_ecommerce.Models.Entities;

namespace crombie_ecommerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } 
        public decimal Total { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
