using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
{
    public class OrderService
    {
        private readonly ShopContext _context;

        public OrderService(ShopContext context)
        {
            _context = context;
        }


        //create a order
        public async Task<Order> CreateOrder(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = orderDto.UserId,
                OrderDate = orderDto.OrderDate,
                Status = orderDto.Status,
                TotalAmount = orderDto.OrderDetails?.Sum(od => od.Subtotal) ?? 0m,
                ShippingAddress = orderDto.ShippingAddress,
                PaymentMethod = orderDto.PaymentMethod,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        
        }

        //get order by id
        public async Task<Order> GetOrderById(Guid orderId) 
        {
            return await _context.Orders.FindAsync(orderId);
        }

        //get all orders from an user id
        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId) 
        {
            return await _context.Orders.Where(o => o.UserId == userId).Include(o=>o.OrderDetails).ToListAsync();
        }

        //update order status
        public async Task<Order> UpdateOrder(Guid id,[FromBody] OrderDto orderdto)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            // Update fields
            existingOrder.Status = orderdto.Status;
            existingOrder.ShippingAddress = orderdto.ShippingAddress;
            existingOrder.PaymentMethod = orderdto.PaymentMethod;

            await _context.SaveChangesAsync();
            return existingOrder;
        }
        public async Task RecalculateOrderTotal(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.TotalAmount = order.OrderDetails?.Sum(od => od.Subtotal) ?? 0m;
            await _context.SaveChangesAsync();
        }
        //delete order
        public async Task<bool> ArchiveMethod(Guid orderId, string processedBy = "Unregistered")
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return false;

            var historyOrder = new HistoryOrder
            {
                OriginalId = order.OrderId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = processedBy,
                EntityJson = order.SerializeToJson()
            };

            _context.HistoryOrders.Add(historyOrder);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
