using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class OrderDetailSeed : IEntityTypeConfiguration<OrderDetail>
    {
        public static readonly Guid AlexDetail = Guid.Parse("1d2e3f4a-5b6c-7d8e-9f0a-1b2c3d4e5f6a");
        public static readonly Guid MariaDetail = Guid.Parse("2e3f4a5b-6c7d-8e9f-0a1b-2c3d4e5f6a7b");
        public static readonly Guid JohnDetail = Guid.Parse("3f4a5b6c-7d8e-9f0a-1b2c-3d4e5f6a7b8c");
        public static readonly Guid EmmaDetail = Guid.Parse("4a5b6c7d-8e9f-0a1b-2c3d-4e5f6a7b8c9d");
        public static readonly Guid LiamDetail = Guid.Parse("5b6c7d8e-9f0a-1b2c-3d4e-5f6a7b8c9d0e");

        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasData(
                new OrderDetail
                {
                    DetailId = AlexDetail,
                    Quantity = 1,
                    Price = 999.99m,
                    OrderId = OrderSeed.AlexOrder,
                    ProductId = ProductSeed.SmartphoneX
                },
                new OrderDetail
                {
                    DetailId = MariaDetail,
                    Quantity = 1,
                    Price = 129.95m,
                    OrderId = OrderSeed.MariaOrder,
                    ProductId = ProductSeed.DenimJacket
                },
                new OrderDetail
                {
                    DetailId = JohnDetail,
                    Quantity = 1,
                    Price = 89.99m,
                    OrderId = OrderSeed.JohnOrder,
                    ProductId = ProductSeed.BlenderPro
                },
                new OrderDetail
                {
                    DetailId = EmmaDetail,
                    Quantity = 2,
                    Price = 34.50m,
                    OrderId = OrderSeed.EmmaOrder,
                    ProductId = ProductSeed.YogaMat
                },
                new OrderDetail
                {
                    DetailId = LiamDetail,
                    Quantity = 1,
                    Price = 24.99m,
                    OrderId = OrderSeed.LiamOrder,
                    ProductId = ProductSeed.MysteryNovel
                }
            );
        }
    }
}