using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class BrandSeed : IEntityTypeConfiguration<Brand>
    {
        public static readonly Guid ElectroTech = Guid.Parse("a3b2c1d0-1234-5678-9abc-def012345678");
        public static readonly Guid UrbanWear = Guid.Parse("b4c3d2e1-2345-6789-0abc-def123456789");
        public static readonly Guid HomeEssentials = Guid.Parse("c5d4e3f2-3456-7890-1abc-def23456789a");
        public static readonly Guid SportFlex = Guid.Parse("d6e5f4d3-4567-8901-2abc-def3456789ab");
        public static readonly Guid BookHaven = Guid.Parse("e7f6f5e4-5678-9012-3abc-def456789abc");

        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasData(
                new Brand
                {
                    BrandId = ElectroTech,
                    Name = "ElectroTech",
                    Description = "Consumer electronics innovator",
                    WebsiteUrl = "https://electrotech.com"
                },
                new Brand
                {
                    BrandId = UrbanWear,
                    Name = "UrbanWear",
                    Description = "Contemporary street fashion",
                    WebsiteUrl = "https://urbanwear.style"
                },
                new Brand
                {
                    BrandId = HomeEssentials,
                    Name = "HomeEssentials",
                    Description = "Premium homeware solutions",
                    WebsiteUrl = "https://homeessentials.co"
                },
                new Brand
                {
                    BrandId = SportFlex,
                    Name = "SportFlex",
                    Description = "High-performance athletic gear",
                    WebsiteUrl = "https://sportflex.com"
                },
                new Brand
                {
                    BrandId = BookHaven,
                    Name = "BookHaven",
                    Description = "Literary classics & new releases",
                    WebsiteUrl = "https://bookhaven.store"
                }
            );
        }
    }
}