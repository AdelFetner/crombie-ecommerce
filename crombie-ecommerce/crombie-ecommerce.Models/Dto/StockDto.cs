using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.Models.Dto
{
    public class StockDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
