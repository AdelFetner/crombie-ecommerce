using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class OrderSeed : IEntityTypeConfiguration<Order>
    {
        // Random GUIDs
        public static readonly Guid AlexOrder = Guid.Parse("6d5e4f3a-2b1c-0d9e-8f7a-6b5c4d3e2f1a");
        public static readonly Guid MariaOrder = Guid.Parse("7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b");
        public static readonly Guid JohnOrder = Guid.Parse("8a9b0c1d-2e3f-4a5b-6c7d-8e9f0a1b2c3d");
        public static readonly Guid EmmaOrder = Guid.Parse("9b0c1d2e-3f4a-5b6c-7d8e-9f0a1b2c3d4e");
        public static readonly Guid LiamOrder = Guid.Parse("0c1d2e3f-4a5b-6c7d-8e9f-0a1b2c3d4e5f");

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasData(
                new Order
                {
                    OrderId = AlexOrder,
                    OrderDate = DateTime.UtcNow.AddDays(-7),
                    Status = "Delivered",
                    TotalAmount = 999.99m, // Matches SmartphoneX price * quantity (1)
                    ShippingAddress = "123 Main St, TechCity",
                    PaymentMethod = "Credit Card",
                    UserId = UserSeed.AlexJohnson
                },
                new Order
                {
                    OrderId = MariaOrder,
                    OrderDate = DateTime.UtcNow.AddDays(-3),
                    Status = "Processing",
                    TotalAmount = 129.95m, // DenimJacket price * quantity (1)
                    ShippingAddress = "456 Oak St, MetroCity",
                    PaymentMethod = "PayPal",
                    UserId = UserSeed.MariaGomez
                },
                new Order
                {
                    OrderId = JohnOrder,
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    Status = "Shipped",
                    TotalAmount = 89.99m, // BlenderPro price * quantity (1)
                    ShippingAddress = "789 Pine Rd, VillageTown",
                    PaymentMethod = "Debit Card",
                    UserId = UserSeed.JohnDoe
                },
                new Order
                {
                    OrderId = EmmaOrder,
                    OrderDate = DateTime.UtcNow.AddHours(-12),
                    Status = "Pending",
                    TotalAmount = 69.00m, // YogaMat price * quantity (2)
                    ShippingAddress = "La bombonera",
                    PaymentMethod = "Apple Pay",
                    UserId = UserSeed.EmmaWilson
                },
                new Order
                {
                    OrderId = LiamOrder,
                    OrderDate = DateTime.UtcNow.AddHours(-2),
                    Status = "Canceled",
                    TotalAmount = 24.99m, // MysteryNovel price * quantity (1)
                    ShippingAddress = "321 Elm St, Bookville",
                    PaymentMethod = "Google Pay",
                    UserId = UserSeed.LiamBrown
                }
            );
        }
    }
}