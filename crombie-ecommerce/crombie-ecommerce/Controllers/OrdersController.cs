﻿using Microsoft.AspNetCore.Mvc;
using crombie_ecommerce.Models.Models.Entities;
using crombie_ecommerce.Services;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

       
        // GET: api/Orders/5
        [HttpGet("{id}")]
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
        public async Task<ActionResult<Order>> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);
            return Ok(orders);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, [FromBody]Order order)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrder(id, order);
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
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            var createdOrder =  await _orderService.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrderById), new {id = createdOrder.OrderId}, createdOrder);    
            
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await _orderService.DeleteOrder(id);
            return Ok(new { message = "The order was successfully deleted" });
        }

       
    }
}
