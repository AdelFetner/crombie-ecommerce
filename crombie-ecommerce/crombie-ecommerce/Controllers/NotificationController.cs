using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<Notification>> CreateNotification(Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdNotification = await _notificationsService.CreateNotification(notification);
            return CreatedAtAction(nameof(GetAllNotifications), new { id = createdNotification.NotfId }, createdNotification);
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