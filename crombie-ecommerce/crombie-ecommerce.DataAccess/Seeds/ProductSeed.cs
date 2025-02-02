using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        // GUIDs follow BrandSeed/CategorySeed sequential pattern
        public static readonly Guid SmartphoneX = Guid.Parse("f8a7b6c5-6789-0123-4abc-def56789abcd");
        public static readonly Guid DenimJacket = Guid.Parse("e7f6a5b4-5678-9012-3abc-def456789abc");
        public static readonly Guid BlenderPro = Guid.Parse("d6e5f4d3-4567-8901-2abc-def3456789ab");
        public static readonly Guid YogaMat = Guid.Parse("c5d4e3f2-3456-7890-1abc-def23456789a");
        public static readonly Guid MysteryNovel = Guid.Parse("b4c3d2e1-2345-6789-0abc-def123456789");

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    ProductId = SmartphoneX,
                    Name = "ElectroTech X10 Pro",
                    Description = "6.8\" AMOLED, 256GB Storage",
                    Price = 999.99m,
                    BrandId = BrandSeed.ElectroTech, // Links to Brand
                    Image = "smartphone-x10.jpg"
                },
                new Product
                {
                    ProductId = DenimJacket,
                    Name = "Urban Classic Denim Jacket",
                    Description = "Slim-fit washed denim",
                    Price = 129.95m,
                    BrandId = BrandSeed.UrbanWear,
                    Image = "denim-jacket-urban.jpg"
                },
                new Product
                {
                    ProductId = BlenderPro,
                    Name = "Essentials Blender Pro",
                    Description = "1500W 8-Speed Countertop Blender",
                    Price = 89.99m,
                    BrandId = BrandSeed.HomeEssentials,
                    Image = "blender-pro.jpg"
                },
                new Product
                {
                    ProductId = YogaMat,
                    Name = "SportFlex Eco Yoga Mat",
                    Description = "Non-slip 6mm thick mat",
                    Price = 34.50m,
                    BrandId = BrandSeed.SportFlex,
                    Image = "yoga-mat-eco.jpg"
                },
                new Product
                {
                    ProductId = MysteryNovel,
                    Name = "Midnight Library: Special Edition",
                    Description = "Hardcover bestseller novel",
                    Price = 24.99m,
                    BrandId = BrandSeed.BookHaven,
                    Image = "midnight-library.jpg"
                }
            );

            // M2M with Categories (uses existing DbContext config)
            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    j => j.HasData(
                        new { ProductId = SmartphoneX, CategoryId = CategorySeed.Electronics },
                        new { ProductId = DenimJacket, CategoryId = CategorySeed.Apparel },
                        new { ProductId = BlenderPro, CategoryId = CategorySeed.HomeLiving },
                        new { ProductId = YogaMat, CategoryId = CategorySeed.SportsOutdoors },
                        new { ProductId = MysteryNovel, CategoryId = CategorySeed.BooksMedia }
                    )
                );
        }
    }
}