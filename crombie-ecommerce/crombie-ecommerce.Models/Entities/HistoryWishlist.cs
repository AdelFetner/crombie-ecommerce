using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace crombie_ecommerce.Models.Entities
{
    [Table("HistoryWishlists")]
    public class HistoryWishlist : History
    {
        //Does not require any additional fields, data is stored in JSON format
    }
}
