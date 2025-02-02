using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
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
        public async Task<Order> CreateOrder(Order order)
        {
            if (order.OrderDetails != null)
            {
                order.TotalAmount = order.OrderDetails.Sum(od => od.Subtotal);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        //get order by id
        public async Task<Order> GetOrderById(Guid id) 
        {
            return await _context.Orders.FindAsync(id);
        }

        //get all orders from an user id
        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId) 
        {
            return await _context.Orders.Where(o => o.UserId == userId).Include(o=>o.OrderDetails).ToListAsync();
        }

        //update order status
        public async Task<Order> UpdateOrder(Guid id, Order order)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            // Update fields
            existingOrder.Status = order.Status;
            existingOrder.ShippingAddress = order.ShippingAddress;
            existingOrder.PaymentMethod = order.PaymentMethod;

            await _context.SaveChangesAsync();
            return existingOrder;
        }


        // this serves as to recalculate when details change
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
        public async Task DeleteOrder(Guid id) 
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

    }
}
