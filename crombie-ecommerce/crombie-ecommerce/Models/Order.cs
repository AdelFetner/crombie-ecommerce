namespace crombie_ecommerce.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }

        public Guid? UserId { get; set; }
        public User? User { get; set; }
        
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
