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
        public async Task<OrderDetail> CreateDetails(OrderDetail detail)
        {
            _context.OrderDetails.Add(detail);
            _context.SaveChanges();
            return detail;
        }

        //update order detail
        public async Task<bool> UpdateDetails(Guid id, OrderDetail orderd)
        {
            var details = await _context.OrderDetails.FindAsync(id);
            if (details == null) return false;

            details.Quantity = orderd.Quantity;
            details.Price = orderd.Price;
            details.Subtotal = orderd.Subtotal;
            
            await _context.SaveChangesAsync();

            return true;


        }


        //delete order detail
        public async Task DeleteDetail(Guid id)
        {
            var details = await _context.OrderDetails.FindAsync(id);
            if (details != null)
            {
                _context.OrderDetails.Remove(details);
                await _context.SaveChangesAsync();
            }
        }
    }   
}
