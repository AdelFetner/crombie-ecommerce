using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
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

        public async Task<bool> UpdateStockAsync(Guid productId, int quantityChange)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
            {
                return false; 
            }

            stock.Quantity += quantityChange;
            stock.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> GetStockByProductIdAsync(Guid productId)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == productId);
            return stock?.Quantity;
        }

        // verifies if there is enough stock for an order
        public async Task<bool> ValidateStockAsync(List<OrderDetail> orderDetails)
        {
            var productIds = orderDetails.Select(d => d.ProductId).ToList();
            var stockRecords = await _context.Stock
                .Where(s => productIds.Contains(s.ProductId))
                .ToListAsync();

            foreach (var detail in orderDetails)
            {
                var stock = stockRecords.FirstOrDefault(s => s.ProductId == detail.ProductId);
                if (stock == null || stock.Quantity < detail.Quantity)
                {
                    throw new Exception($"Insufficient stock for: {detail.ProductId}");
                }
            }
            return true;
        }

        // process an order and update stock
        public async Task<bool> ProcessOrderAsync(OrderDto orderDto, List<OrderDetail> orderDetails)
        {
            var order = new Order();
            if (!await ValidateStockAsync(orderDetails))
            {
                return false;
            }

            foreach (var detail in orderDetails)
            {
                var stock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == detail.ProductId);
                if (stock != null)
                {
                    stock.Quantity -= detail.Quantity;
                    if (stock.Quantity < 0)
                    {
                        throw new Exception($"Without stock for: {stock.ProductId}");
                    }
                }
                else
                {
                    throw new Exception($"Stock not found for: {detail.ProductId}");
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }

        // cancel an order and return stock
        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found");
            }

            foreach (var detail in order.OrderDetails)
            {
                var stock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == detail.ProductId);
                if (stock != null)
                {
                    stock.Quantity += detail.Quantity;
                }
                else
                {
                    throw new Exception($"Not found product ID: {detail.ProductId}");
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
