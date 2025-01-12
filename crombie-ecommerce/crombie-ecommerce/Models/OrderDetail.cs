namespace crombie_ecommerce.Models
{
    public class OrderDetail
    {
        public Guid DetailsId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }

        public Guid? OrderId { get; set; }        
        public Order Order { get; set; }

        public Guid? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
