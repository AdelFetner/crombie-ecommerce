using System.Collections.Generic;
using System.Threading.Tasks;
using crombie_ecommerce.DTOs;
using crombie_ecommerce.Models;
using crombie_ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationsService _notificationsService;

        public NotificationsController(NotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        // get all notifications
        [HttpGet]
        public async Task<ActionResult<List<Notification>>> GetAllNotifications()
        {
            return Ok(await _notificationsService.GetAllNotifications());
        }

        // create notification
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationDTO notificationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState });
            }
            try
            {
                var notification = new Notification
                {
                    NotfId = Guid.NewGuid(),
                    NotificationType = notificationDto.NotificationType,
                    Message = notificationDto.Message,
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                };
                var createdNotification = await _notificationsService.CreateNotification(notification);
                return Ok(new
                {
                    message = "Notification created successfully.",
                    notification = createdNotification
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // assign notification to wishlist or product
        [HttpPost("assign/{notificationId}")]
        public async Task<ActionResult> AssignNotification(Guid notificationId, [FromQuery] Guid? wishlistId, [FromQuery] Guid? productId)
        {
            if (wishlistId == null && productId == null)
            {
                return BadRequest("A wishlistId or productId must be provided.");
            }

            var success = await _notificationsService.AssignNotification(notificationId, wishlistId, productId);
            if (!success)
            {
                return NotFound("Entity not found.");
            }

            return NoContent();
        }

        // delete notification
        [HttpDelete("{notificationId}")]
        public async Task<ActionResult> DeleteNotification(Guid notificationId)
        {
            var success = await _notificationsService.DeleteNotification(notificationId);
            if (!success)
            {
                return NotFound("Notification not found.");
            }
            return NoContent();
        }
    }
}