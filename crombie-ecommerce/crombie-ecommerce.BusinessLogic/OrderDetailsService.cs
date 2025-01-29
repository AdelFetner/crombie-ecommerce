using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
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
            detail.DetailId = Guid.NewGuid();
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
            return _context.OrderDetails.Include(od => od.Product).Include(od => od.Order).FirstOrDefault(od => od.DetailId == id);
        }

        //update order detail
        public async Task<OrderDetail> UpdateDetails(Guid id, OrderDetail orderd)
        {
            var details = await _context.OrderDetails.FindAsync(id);
            

            details.Quantity = orderd.Quantity;
            details.Price = orderd.Price;
            details.Subtotal = orderd.Subtotal;
            
            _context.OrderDetails.Update(details);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return details;


        }


        //delete order detail
        public async Task<bool> ArchiveMethod(Guid detailId, string processedBy = "Unregistered")
        {
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.DetailId == detailId);

            if (orderDetail == null)
                return false;

            var historyOrderDetail = new HistoryOrderDetails
            {
                OriginalId = orderDetail.DetailId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = processedBy,
                EntityJson = orderDetail.SerializeToJson()
            };

            _context.HistoryOrderDetails.Add(historyOrderDetail);
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return true;
        }
    }   
}
