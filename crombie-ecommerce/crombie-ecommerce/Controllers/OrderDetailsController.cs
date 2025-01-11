using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using crombie_ecommerce.Services;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailsService _orderDetailsService;

        public OrderDetailsController(OrderDetailsService orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllOrderDetails()
        {
            var details = await _orderDetailsService.GetAllDetails();
            return Ok(details);
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetailById(Guid id)
        {
            var orderDetail = await _orderDetailsService.GetDetailsById(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(orderDetail);
        }

        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(Guid id, [FromBody]OrderDetail orderDetail)
        {
            try
            {
                var updatedOrderDetail = _orderDetailsService.UpdateDetails(id, orderDetail);
                return Ok(updatedOrderDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
            var createdDetail = _orderDetailsService.CreateDetails(orderDetail);
            return CreatedAtAction(nameof(GetOrderDetailById), new {id = createdDetail.Id}, createdDetail);
            
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(Guid id)
        {
            var orderDetail =  _orderDetailsService.DeleteDetails(id);
            if (orderDetail == null)
            {
                return NotFound();
            }


            return NoContent();
        }

    }
}
