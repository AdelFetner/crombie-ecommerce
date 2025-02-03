using crombie_ecommerce.Models.Entities;
using System.Text.Json.Serialization;


namespace crombie_ecommerce.Models.Dto
{
    public class OrderDto
    {
        public Guid? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();


    }
}
