using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

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
            detail.DetailsId = Guid.NewGuid();
            detail.Subtotal= detail.Quantity * detail.Price;

            _context.OrderDetails.Add(detail);
            _context.SaveChanges();
            return detail;
        }

        //get all order detail  
        public async Task<IEnumerable<OrderDetail>> GetAllDetails()
        {
            return  _context.OrderDetails.Include(od => od.Product).Include(od => od.Order).ToList();
        }

        //get order detail by id
        public async Task<OrderDetail> GetDetailsById(Guid id)
        {
            return _context.OrderDetails.Include(od => od.Product).Include(od => od.Order).FirstOrDefault(od => od.DetailsId == id);
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
        public async Task DeleteDetails(Guid id)
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
