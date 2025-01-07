using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Services
{
    public class OrderService
    {
        private readonly ShopContext _context;

        public OrderService(ShopContext context)
        {
            _context = context;
        }


        //create a order
        public async Task<Order> CreateOrder (Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        //get order by id
        public async Task<Order> GetOrderById(Guid id) 
        {
            return await _context.Orders.FindAsync(id);
        }

        //get all orders from an id
        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId) 
        {
            return await _context.Orders.Where(o => o.UserId == userId).Include(o=>o.OrderDetails).ToListAsync();
        }

        //update order status
        public async Task<bool> UpdateOrderStatus(Guid id, string newStatus) 
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();

            return true;
        
                
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
