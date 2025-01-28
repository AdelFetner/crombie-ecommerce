using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class TagSeed
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new Tag
                {
                    TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0001"), // Sports Tag
                    Name = "Sports",
                    Description = "Tags for sports-related products.",
                    WishlistId = Guid.Parse("1111aaaa-bbbb-cccc-dddd-eeeeffff1111") // John's Wishlist
                },
                new Tag
                {
                    TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0002"), // Electronics Tag
                    Name = "Electronics",
                    Description = "Tags for electronics and gadgets.",
                    WishlistId = Guid.Parse("2222bbbb-cccc-dddd-eeee-ffff1111aaaa") // Jane's Wishlist
                },
                new Tag
                {
                    TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0003"), // Books Tag
                    Name = "Books",
                    Description = "Tags for books, novels, and literature.",
                    WishlistId = Guid.Parse("3333cccc-dddd-eeee-ffff-1111aaaabbbb") // Alice's Wishlist
                },
                new Tag
                {
                    TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0004"), // Gaming Tag
                    Name = "Gaming",
                    Description = "Tags for gaming consoles and accessories.",
                    WishlistId = Guid.Parse("4444dddd-eeee-ffff-1111-aaaabbbbcccc") // Bob's Wishlist
                },
                new Tag
                {
                    TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0005"), // Music Tag
                    Name = "Music",
                    Description = "Tags for musical instruments and gear.",
                    WishlistId = Guid.Parse("5555eeee-ffff-1111-aaaa-bbbbccccdddd") // Charlie's Wishlist
                },
                new Tag
                {
                    TagId = Guid.Parse("aaaa1111-bbbb-cccc-dddd-eeeeffff0006"), // Premium Tag
                    Name = "Premium",
                    Description = "Tags for high-end or premium products.",
                    WishlistId = null // Not tied to a specific wishlist for flexibility
                }
                );
        }
    }
}
