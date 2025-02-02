using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public static readonly Guid Electronics = Guid.Parse("f8a7b6c5-6789-0123-4abc-def56789abcd");
        public static readonly Guid Apparel = Guid.Parse("e7f6a5b4-5678-9012-3abc-def456789abc");
        public static readonly Guid HomeLiving = Guid.Parse("d6e5f4d3-4567-8901-2abc-def3456789ab");
        public static readonly Guid SportsOutdoors = Guid.Parse("c5d4e3f2-3456-7890-1abc-def23456789a");
        public static readonly Guid BooksMedia = Guid.Parse("b4c3d2e1-2345-6789-0abc-def123456789");

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    CategoryId = Electronics,
                    Name = "Electronics",
                    Description = "Devices, gadgets, and tech accessories"
                },
                new Category
                {
                    CategoryId = Apparel,
                    Name = "Apparel",
                    Description = "Clothing and fashion items"
                },
                new Category
                {
                    CategoryId = HomeLiving,
                    Name = "Home & Living",
                    Description = "Furniture and home decor"
                },
                new Category
                {
                    CategoryId = SportsOutdoors,
                    Name = "Sports & Outdoors",
                    Description = "Athletic gear and outdoor equipment"
                },
                new Category
                {
                    CategoryId = BooksMedia,
                    Name = "Books & Media",
                    Description = "Physical and digital media"
                }
            );
        }
    }
}