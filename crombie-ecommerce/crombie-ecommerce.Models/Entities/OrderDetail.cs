﻿using Newtonsoft.Json;

namespace crombie_ecommerce.Models.Entities
{
    public class OrderDetail 
    {
        public Guid DetailId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Subtotal { get; set;}

        public Guid OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }

        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
        public Guid Id => DetailId;
        public string SerializeToJson()
        { 
            return JsonConvert.SerializeObject(this);
        }
    }
}
