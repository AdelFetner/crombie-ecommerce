using Microsoft.AspNetCore.Mvc;
using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.Models.Entities;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using crombie_ecommerce.Models.Dto;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly StockService _stockService;

        public OrdersController(OrderService orderService, StockService stockService)
        {
            _orderService = orderService;
            _stockService = stockService;
        }

       
        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // GET: api/Orders
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);
            return Ok(orders);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutOrder(Guid id, [FromBody]OrderDto orderDto)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrder(id, orderDto);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> PostOrder([FromBody]OrderDto order)
        {
            // validate stock before creating the order
            if (!await _stockService.ValidateStockAsync(order.OrderDetails.ToList()))
            {
                return BadRequest(new { message = "Insufficient stock." });
            }

            // process the order and update stock
            if (!await _stockService.ProcessOrderAsync(order,order.OrderDetails.ToList()))
            {
                return BadRequest(new { message = "Failed to process the order." });
            }

            // create the order
            var createdOrder = await _orderService.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAndArchive(Guid id)
        {
            var cancelSuccess = await _stockService.CancelOrderAsync(id);
            if (!cancelSuccess)
            {
                return NotFound(new { message = "Order not found." });
            }

            var archiveSuccess = await _orderService.ArchiveMethod(id, "Unregistered");
            if (!archiveSuccess)
            {
                return BadRequest(new { message = "Order stock restored but failed to delete the order." });
            }

            return Ok(new { message = "Order canceled, stock restored, and deleted successfully." });
        }
    }
}
