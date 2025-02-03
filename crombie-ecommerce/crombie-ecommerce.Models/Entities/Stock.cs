using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace crombie_ecommerce.Models.Entities
    {
            [Table("Stock")]
        public class Stock
        {
            [Key]
            public Guid StockId { get; set; }

            [Required]
            public Guid ProductId { get; set; }

            public int Quantity { get; set; }

            public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

            [ForeignKey("ProductId")]
            public virtual Product Product { get; set; }
        }
    }