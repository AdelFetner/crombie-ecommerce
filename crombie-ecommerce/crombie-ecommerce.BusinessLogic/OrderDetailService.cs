using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
{
    public class OrderDetailService
    {
        private readonly ShopContext _context;
        private readonly OrderService _orderService;

        public OrderDetailService(ShopContext context, OrderService orderService) 
        {
            _orderService = orderService;
            _context = context;
        }

        //create order detail
        public async Task<OrderDetail> CreateDetails(OrderDetail detail)
        {
            detail.DetailId = Guid.NewGuid();
            detail.Subtotal = detail.Quantity * detail.Price;

            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();
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
            return _context.OrderDetails.Include(od => od.Product).Include(od => od.Order).FirstOrDefault(od => od.DetailId == id);
        }

        //update order detail
        public async Task<OrderDetail> UpdateDetails(Guid id, OrderDetail orderd)
        {
            var details = await _context.OrderDetails.FindAsync(id);

            details.Quantity = orderd.Quantity;
            details.Price = orderd.Price;
            details.Subtotal = details.Quantity * details.Price;

            _context.OrderDetails.Update(details);
            await _context.SaveChangesAsync();

            await _orderService.RecalculateOrderTotal(details.OrderId);
            return details;
        }

        //delete order detail
        public async Task DeleteDetails(Guid id)
        {
            var details = await _context.OrderDetails.FindAsync(id);
            if (details != null)
            {
                var orderId = details.OrderId;
                _context.OrderDetails.Remove(details);
                await _context.SaveChangesAsync();

                await _orderService.RecalculateOrderTotal(orderId);
            }
        }
    }   
}
