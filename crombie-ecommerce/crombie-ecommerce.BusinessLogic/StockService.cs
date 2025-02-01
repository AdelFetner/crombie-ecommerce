using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.BusinessLogic
{
    public class StockService
    {
        private readonly ShopContext _context;

        public StockService(ShopContext context)
        {
            _context = context;
        }

        // verifies if there is enough stock for an order
        public async Task<bool> ValidateStockAsync(List<OrderDetail> orderDetails)
        {
            var productId = orderDetails.Select(d => d.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productId.Contains(p.ProductId))
                .ToListAsync();

            foreach (var detail in orderDetails)
            {
                var product = products.FirstOrDefault(p => p.ProductId == detail.ProductId);
                if (product == null || product.Stock < detail.Quantity)
                {
                    return false;
                }
            }
            return true;
        }


        // process an order and update stock
        public async Task<bool> ProcessOrderAsync(Order order, List<OrderDetail> orderDetails)

        {
            if (!await ValidateStockAsync(orderDetails))
            {
                return false;
            }

            foreach (var detail in orderDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product != null)
                {
                    product.Stock -= detail.Quantity;

                    if (product.Stock == 0)
                    {
                        Console.WriteLine($"Product {product.Name} without stock");
                    }
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }

        // cancel an order and return stock
        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;

            foreach (var detail in order.OrderDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product != null)
                {
                    product.Stock += detail.Quantity;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
