using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class WishlistSeed
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new Wishlist
                {
                    WishlistId = Guid.Parse("1111aaaa-bbbb-cccc-dddd-eeeeffff1111"), // John's Wishlist
                    Name = "John's Sports Wishlist",
                    Description = "Wishlist for sports equipment and accessories.",
                    UserId = Guid.Parse("a1b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6"), // John Doe
                    Products = new List<Product>
                    {
                        new Product { ProductId = Guid.Parse("7a8b9c0d-1e2f-3a4b-5c6d-7e8f9a0b1c2d") }, // Nike Air Max 90
                        new Product { ProductId = Guid.Parse("6c5d4e3f-2a1b-9c0d-8f7e-6b5a4c3d2e1f") }  // Adidas Ultraboost
                    },
                    TagsId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0001"), // Sports Tag
                    Tags = new List<Tag>
                    {
                        new Tag { TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0001") } // Sports Tag
                    },
                    NotfId = null,
                    Notifications = new List<Notification>()
                },
                new Wishlist
                {
                    WishlistId = Guid.Parse("2222bbbb-cccc-dddd-eeee-ffff1111aaaa"), // Jane's Wishlist
                    Name = "Jane's Electronics Wishlist",
                    Description = "Wishlist for gadgets and electronics.",
                    UserId = Guid.Parse("b2c3d4e5-f6a7-b8c9-d0e1-f2a3b4c5d6a1"), // Jane Smith
                    Products = new List<Product>
                    {
                        new Product { ProductId = Guid.Parse("8f7e6b5a-4c3d-2e1f-9a8b7c6d5e4f") }, // Samsung Galaxy S23
                        new Product { ProductId = Guid.Parse("4c3d2e1f-8b7a-6c5d-4f3e-2d1c9a8b7c6d") }  // Apple iPhone 14 Pro
                    },
                    TagsId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0002"), // Electronics Tag
                    Tags = new List<Tag>
                    {
                        new Tag { TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0002") } // Electronics Tag
                    },
                    NotfId = null,
                    Notifications = new List<Notification>()
                },
                new Wishlist
                {
                    WishlistId = Guid.Parse("3333cccc-dddd-eeee-ffff-1111aaaabbbb"), // Alice's Wishlist
                    Name = "Alice's Book Wishlist",
                    Description = "Wishlist for books and literature.",
                    UserId = Guid.Parse("c3d4e5f6-a7b8-c9d0-e1f2-a3b4c5d6a1b2"), // Alice Brown
                    Products = new List<Product>
                    {
                        new Product { ProductId = Guid.Parse("1d9c8b7a-6c5d-4e3f-2a1b-9c0d8f7e6b5a") }, // Creative Writing Guide
                        new Product { ProductId = Guid.Parse("2e1f9a8b-7c6d-5b4a-3c2d-1e9f8d7c6b5a") }  // Historical Novel Bundle
                    },
                    TagsId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0003"), // Books Tag
                    Tags = new List<Tag>
                    {
                        new Tag { TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0003") } // Books Tag
                    },
                    NotfId = null,
                    Notifications = new List<Notification>()
                },
                new Wishlist
                {
                    WishlistId = Guid.Parse("4444dddd-eeee-ffff-1111-aaaabbbbcccc"), // Bob's Wishlist
                    Name = "Bob's Gaming Wishlist",
                    Description = "Wishlist for gaming consoles and accessories.",
                    UserId = Guid.Parse("d4e5f6a7-b8c9-d0e1-f2a3-b4c5d6a1b2c3"), // Bob Johnson
                    Products = new List<Product>
                    {
                        new Product { ProductId = Guid.Parse("5b4a3c2d-1e9f-8d7c-6b5a4c3d2e1f") }, // Sony PlayStation 5
                        new Product { ProductId = Guid.Parse("3a2b1c9d-8f7e-6b5a-4c3d2e1f9a8b") }  // Nintendo Switch OLED
                    },
                    TagsId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0004"), // Gaming Tag
                    Tags = new List<Tag>
                    {
                        new Tag { TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0004") } // Gaming Tag
                    },
                    NotfId = null,
                    Notifications = new List<Notification>()
                },
                new Wishlist
                {
                    WishlistId = Guid.Parse("5555eeee-ffff-1111-aaaa-bbbbccccdddd"), // Charlie's Wishlist
                    Name = "Charlie's Music Wishlist",
                    Description = "Wishlist for musical instruments and gear.",
                    UserId = Guid.Parse("e5f6a7b8-c9d0-e1f2-a3b4-c5d6a1b2c3d4"), // Charlie Davis
                    Products = new List<Product>
                    {
                        new Product { ProductId = Guid.Parse("1c9d8f7e-6b5a-4c3d-2e1f-9a8b7c6d5e4f") }, // Yamaha Acoustic Guitar
                        new Product { ProductId = Guid.Parse("9a8b7c6d-5e4f-3a2b-1c9d-8f7e6b5a4c3d") }  // Bose Headphones
                    },
                    TagsId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0005"), // Music Tag
                    Tags = new List<Tag>
                    {
                        new Tag { TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0005") } // Music Tag
                    },
                    NotfId = null,
                    Notifications = new List<Notification>()
                }
            );
        }
    }
}
