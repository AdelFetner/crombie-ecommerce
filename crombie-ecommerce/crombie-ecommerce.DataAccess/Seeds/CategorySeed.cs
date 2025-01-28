using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    CategoryId = Guid.Parse("f1a1a1f0-b2b2-c3c3-d4d4-e5e5f6f6a7a7"),
                    Name = "Footwear",
                    Description = "Shoes, sandals, boots, and other types of footwear for various occasions."
                },
                new Category
                {
                    CategoryId = Guid.Parse("a7a7f6f6-e5e5-d4d4-c3c3-b2b2f1a1a1f0"),
                    Name = "Electronics",
                    Description = "Devices such as smartphones, laptops, and home appliances."
                },
                new Category
                {
                    CategoryId = Guid.Parse("b3b3c4c4-d5d5-e6e6-f7f7-a8a8a9a9b0b0"),
                    Name = "Clothing",
                    Description = "Apparel including shirts, pants, dresses, and outerwear."
                },
                new Category
                {
                    CategoryId = Guid.Parse("c1c1d2d2-e3e3-f4f4-a5a5-b6b6b7b7c8c8"),
                    Name = "Sports Equipment",
                    Description = "Gear and equipment for sports and outdoor activities."
                },
                new Category
                {
                    CategoryId = Guid.Parse("d4d4e5e5-f6f6-a7a7-b8b8-c9c9c0c0d1d1"),
                    Name = "Accessories",
                    Description = "Items like watches, bags, sunglasses, and jewelry."
                },
                new Category
                {
                    CategoryId = Guid.Parse("e6e6f7f7-a8a8-b9b9-c0c0-d1d1d2d2e3e3"),
                    Name = "Audio",
                    Description = "Headphones, speakers, and other audio equipment."
                }
            );
        }
    }
}