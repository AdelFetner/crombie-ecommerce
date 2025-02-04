using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
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
        public async Task<OrderDetail> CreateDetails(OrderDetailsDto detail)
        {
            var details= new OrderDetail
            {
                DetailId = Guid.NewGuid(),
                OrderId = detail.OrderId,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity, 
                Price = detail.Price,
                Subtotal = detail.Subtotal,
            };

            _context.OrderDetails.Add(details);
            await _context.SaveChangesAsync();
            return details;
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
        public async Task<OrderDetail> UpdateDetails(Guid id, OrderDetailsDto orderDetailsDto)
        {
            var details = await _context.OrderDetails.FindAsync(id);

            details.Quantity = orderDetailsDto.Quantity;
            details.Price = orderDetailsDto.Price;
            details.Subtotal = details.Quantity * details.Price;

            _context.OrderDetails.Update(details);
            await _context.SaveChangesAsync();

            await _orderService.RecalculateOrderTotal(details.OrderId);
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
