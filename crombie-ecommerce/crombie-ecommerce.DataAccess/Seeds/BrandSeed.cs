using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class BrandSeed : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasData(
                new Brand
                {
                    BrandId = Guid.Parse("d5a6e5f4-3a2b-1c9d-8f7e-6b5a4c3d2e1f"),
                    Name = "Nike",
                    Description = "Global sports and athletic wear manufacturer",
                    WebsiteUrl = "https://www.nike.com"
                },
                new Brand
                {
                    BrandId = Guid.Parse("9f8e7d6c-5b4a-3c2d-1e9f-8d7c6b5a4c3d"),
                    Name = "Samsung",
                    Description = "Leading electronics manufacturer",
                    WebsiteUrl = "https://www.samsung.com"
                },
                new Brand
                {
                    BrandId = Guid.Parse("4c3d2e1f-8b7a-6c5d-4f3e-2d1c9a8b7c6d"),
                    Name = "Apple",
                    Description = "Premium technology and electronics company",
                    WebsiteUrl = "https://www.apple.com"
                },
                new Brand
                {
                    BrandId = Guid.Parse("2e1c9a8b-7d6c-5b4a-3f2e-1d9c8b7a6c5d"),
                    Name = "Adidas",
                    Description = "Sports and lifestyle brand",
                    WebsiteUrl = "https://www.adidas.com"
                },
                new Brand
                {
                    BrandId = Guid.Parse("1c9d8f7e-6b5a-4c3d-2e1f-9a8b7c6d5e4f"),
                    Name = "Sony",
                    Description = "Entertainment and electronics innovator",
                    WebsiteUrl = "https://www.sony.com"
                }
            );
        }
    }
}
