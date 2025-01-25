using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Services
{
    public class NotificationService
    {
        private readonly ShopContext _context;

        public NotificationService(ShopContext context)
        {
            _context = context;
        }

        // method to get all notifications
        public async Task<List<Notification>> GetAllNotifications()
        {
            return await _context.Notifications
                .Include(n => n.Wishlist)
                .Include(n => n.Product)
                .ToListAsync();
        }

        // method to get all notifications by wishlist id
        public async Task<List<Notification>> GetNotificationsByWishlistId(Guid wishlistId)
        {
            return await _context.Notifications
                .Where(n => n.WishlistId == wishlistId)
                .Include(n => n.Product)
                .ToListAsync();
        }

        // create notification
        public async Task<Notification> CreateNotification(Notification notification)
        {
            notification.CreatedDate = DateTime.UtcNow; // set created date to current date
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        // Mark notification as read
        public async Task<bool> MarkAsRead(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        //method to assign notification to wishlist or product
        public async Task<bool> AssignNotification(Guid notificationId, Guid? wishlistId, Guid? productId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return false;
            }

            if (wishlistId.HasValue)
            {
                var wishlist = await _context.Wishlists.FindAsync(wishlistId.Value);
                if (wishlist == null)
                {
                    return false;
                }
                notification.WishlistId = wishlistId.Value;
            }

            if (productId.HasValue)
            {
                var product = await _context.Products.FindAsync(productId.Value);
                if (product == null)
                {
                    return false;
                }
                notification.ProductId = productId.Value;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // delete notification
        public async Task<bool> DeleteNotification(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}