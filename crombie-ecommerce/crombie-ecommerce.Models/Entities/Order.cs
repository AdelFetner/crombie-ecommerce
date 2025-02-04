using Newtonsoft.Json;
﻿using System.ComponentModel.DataAnnotations.Schema;

namespace crombie_ecommerce.Models.Entities
{
    public class Order 
    {
        public Guid OrderId { get; set; }

        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }

        public decimal TotalAmount { get; set; }

        [NotMapped]
        [JsonIgnore]
        public decimal CalculatedTotal => OrderDetails?.Sum(od => od.Subtotal) ?? 0m;
        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; }

        public Guid? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
        public Guid Id => OrderId;
        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
