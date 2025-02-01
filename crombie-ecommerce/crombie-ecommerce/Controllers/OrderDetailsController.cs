using Microsoft.AspNetCore.Mvc;
using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.Models.Entities;
using Interfaces;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllOrderDetails()
        {
            var details = await _orderDetailsService.GetAllDetails();
            return Ok(details);
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> PutOrderDetail(Guid id, [FromBody]OrderDetail orderDetail)
        {
            try
            {
                var updatedOrderDetail = await _orderDetailsService.UpdateDetails(id, orderDetail);
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
        [Authorize]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
            var createdDetail = await _orderDetailsService.CreateDetails(orderDetail);
            return CreatedAtAction(nameof(GetOrderDetailById), new {id = createdDetail.DetailId}, createdDetail);
            
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAndArchive(Guid id)
        {
            var success = await _orderDetailsService.ArchiveMethod(id, "Unregistered");
            if (!success)
            {
                return NotFound(new { message = "Order detail not found." });
            }
            return Ok(new { message = "Order detail deleted successfully." });
        }

    }
}
