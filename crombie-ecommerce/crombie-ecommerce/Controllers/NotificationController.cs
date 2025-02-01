using crombie_ecommerce.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly ShopContext _context;

        public NotificationController(NotificationService notificationService, ShopContext context)
        {
            _notificationService = notificationService;
            _context = context;
        }

        // get all notifications
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Notification>>> GetAllNotifications()
        {
            return Ok(await _notificationService.GetAllNotifications());
        }

        // create notification
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationDTO notificationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { errors = ModelState });
                }

                var wishlistExists = await _context.Wishlists.AnyAsync(w => w.WishlistId == notificationDto.WishlistId);

                if (!wishlistExists)
                {
                    return BadRequest("Invalid WishlistId");
                }

                var notification = new Notification
                {
                    NotfId = Guid.NewGuid(),
                    NotificationType = notificationDto.NotificationType,
                    Message = notificationDto.Message,
                    ProductId = notificationDto.ProductId,
                    WishlistId = notificationDto.WishlistId,
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                };

                var createdNotification = await _notificationService.CreateNotification(notification);
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
        [Authorize]
        public async Task<ActionResult> AssignNotification(Guid notificationId, [FromQuery] Guid? wishlistId, [FromQuery] Guid? productId)
        {
            if (wishlistId == null && productId == null)
            {
                return BadRequest("A wishlistId or productId must be provided.");
            }

            var success = await _notificationService.AssignNotification(notificationId, wishlistId, productId);
            if (!success)
            {
                return NotFound("Entity not found.");
            }

            return NoContent();
        }

        // delete notification
        [HttpDelete("{notificationId}")]
        [Authorize]
        public async Task<ActionResult> DeleteNotification(Guid notificationId)
        {
            var success = await _notificationService.DeleteNotification(notificationId);
            if (!success)
            {
                return NotFound("Notification not found.");
            }
            return NoContent();
        }
    }
}