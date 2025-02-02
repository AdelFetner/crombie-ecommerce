using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class NotificationSeed : IEntityTypeConfiguration<Notification>
    {
        public static readonly Guid PriceDropAlert = Guid.Parse("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e");
        public static readonly Guid RestockNotice = Guid.Parse("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f");
        public static readonly Guid WishlistUpdate = Guid.Parse("3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a");
        public static readonly Guid OrderShipped = Guid.Parse("4e5f6a7b-8c9d-0e1f-2a3b-4c5d6e7f8a9b");
        public static readonly Guid SpecialOffer = Guid.Parse("5f6a7b8c-9d0e-1f2a-3b4c-5d6e7f8a9b0c");

        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasData(
                new Notification
                {
                    NotificationId = PriceDropAlert,
                    NotificationType = "Price Alert",
                    Message = "Price dropped on ElectroTech X10 Pro!",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    IsRead = false,
                    ProductId = ProductSeed.SmartphoneX,
                    WishlistId = WishlistSeed.TechWishlist
                },
                new Notification
                {
                    NotificationId = RestockNotice,
                    NotificationType = "Restock",
                    Message = "Urban Denim Jacket back in stock",
                    CreatedDate = DateTime.UtcNow.AddHours(-12),
                    IsRead = true,
                    ProductId = ProductSeed.DenimJacket,
                    WishlistId = WishlistSeed.FashionWishlist
                },
                new Notification
                {
                    NotificationId = WishlistUpdate,
                    NotificationType = "Wishlist Update",
                    Message = "New item added to Kitchen Upgrades",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    ProductId = ProductSeed.BlenderPro,
                    WishlistId = WishlistSeed.KitchenWishlist
                },
                new Notification
                {
                    NotificationId = SpecialOffer,
                    NotificationType = "Special Offer",
                    Message = "20% off all books this week!",
                    CreatedDate = DateTime.UtcNow.AddHours(-1),
                    ProductId = ProductSeed.MysteryNovel,
                    WishlistId = WishlistSeed.ReadingWishlist
                }
            );
        }
    }
}