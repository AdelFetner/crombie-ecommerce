using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;

namespace crombie_ecommerce.Services
{
    public class OrderDetailsService
    {
        private readonly ShopContext _context;

        public OrderDetailsService(ShopContext context) 
        {
            _context = context;
        }

        //create order detail
        //update order detail
        //delete order detail
    }
}
