using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.Models.Entities
{
    [Table("HistoryOrders")]
    public class HistoryOrder : History
    {
    }
}
