using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class ProductSeed
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Product
                {
                    ProductId = Guid.Parse("a1a1a1a1-b2b2-c3c3-d4d4-e5e5f6f6a7a7"), // Air Max 90
                    Name = "Air Max 90",
                    Description = "Classic Nike sneakers with premium cushioning.",
                    Price = 129.99m,
                    BrandId = Guid.Parse("d5a6e5f4-3a2b-1c9d-8f7e-6b5a4c3d2e1f"), // Nike
                    Image = "airmax90.jpg",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            CategoryId = Guid.Parse("f1a1a1f0-b2b2-c3c3-d4d4-e5e5f6f6a7a7"), // Footwear
                            Name = "Footwear"
                        }
                    }
                },
                new Product
                {
                    ProductId = Guid.Parse("b2b2b2b2-c3c3-d4d4-e5e5-f6f6a7a7f1a1"), // Galaxy S22
                    Name = "Galaxy S22",
                    Description = "Samsung's latest flagship smartphone with cutting-edge features.",
                    Price = 999.99m,
                    BrandId = Guid.Parse("9f8e7d6c-5b4a-3c2d-1e9f-8d7c6b5a4c3d"), // Samsung
                    Image = "galaxys22.jpg",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            CategoryId = Guid.Parse("a7a7f6f6-e5e5-d4d4-c3c3-b2b2f1a1a1f0"), // Electronics
                            Name = "Electronics"
                        }
                    }
                },
                new Product
                {
                    ProductId = Guid.Parse("c3c3c3c3-d4d4-e5e5-f6f6-a7a7f1a1b2b2"), // MacBook Pro 16
                    Name = "MacBook Pro 16",
                    Description = "Apple's high-performance laptop for professionals.",
                    Price = 2499.99m,
                    BrandId = Guid.Parse("4c3d2e1f-8b7a-6c5d-4f3e-2d1c9a8b7c6d"), // Apple
                    Image = "macbookpro16.jpg",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            CategoryId = Guid.Parse("a7a7f6f6-e5e5-d4d4-c3c3-b2b2f1a1a1f0"), // Electronics
                            Name = "Electronics"
                        }
                    }
                },
                new Product
                {
                    ProductId = Guid.Parse("d4d4d4d4-e5e5-f6f6-a7a7-b2b2f1c3a3a3"), // Ultraboost 22
                    Name = "Ultraboost 22",
                    Description = "Adidas running shoes offering unmatched comfort and energy return.",
                    Price = 180.00m,
                    BrandId = Guid.Parse("2e1c9a8b-7d6c-5b4a-3f2e-1d9c8b7a6c5d"), // Adidas
                    Image = "ultraboost22.jpg",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            CategoryId = Guid.Parse("f1a1a1f0-b2b2-c3c3-d4d4-e5e5f6f6a7a7"), // Footwear
                            Name = "Footwear"
                        }
                    }
                },
                new Product
                {
                    ProductId = Guid.Parse("e5e5e5e5-f6f6-a7a7-b2b2-c3c3d4f1a2a2"), // WH-1000XM5 Headphones
                    Name = "WH-1000XM5 Headphones",
                    Description = "Sony's industry-leading noise-canceling headphones.",
                    Price = 399.99m,
                    BrandId = Guid.Parse("1c9d8f7e-6b5a-4c3d-2e1f-9a8b7c6d5e4f"), // Sony
                    Image = "wh1000xm5.jpg",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            CategoryId = Guid.Parse("e6e6f7f7-a8a8-b9b9-c0c0-d1d1d2d2e3e3"), // Audio
                            Name = "Audio"
                        }
                    }
                },
                new Product
                {
                    ProductId = Guid.Parse("f6f6f6f6-a7a7-b2b2-c3c3-d4d4e5f1b3b3"), // Pro Gym Set
                    Name = "Pro Gym Set",
                    Description = "High-quality gym equipment set for home workouts.",
                    Price = 499.99m,
                    BrandId = Guid.Parse("2e1c9a8b-7d6c-5b4a-3f2e-1d9c8b7a6c5d"), // Adidas
                    Image = "progymset.jpg",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            CategoryId = Guid.Parse("c1c1d2d2-e3e3-f4f4-a5a5-b6b6b7b7c8c8"), // Sports Equipment
                            Name = "Sports Equipment"
                        }
                    }
                }
            );
        }
    }
}
