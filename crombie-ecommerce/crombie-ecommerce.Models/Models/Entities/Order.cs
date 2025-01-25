using System.Text.Json.Serialization;

namespace crombie_ecommerce.Models.Models.Entities
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
        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
