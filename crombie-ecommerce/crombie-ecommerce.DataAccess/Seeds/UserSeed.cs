using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    UserId = Guid.Parse("a1b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6"), // John Doe
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Password = "password123",
                    Address = "123 Elm Street, Springfield, USA",
                    IsVerified = true,
                    Image = "john_doe.jpg",
                    Wishlists = new List<Wishlist>(), // Can be populated later
                    Orders = new List<Order>() // Can be populated later
                },
                new User
                {
                    UserId = Guid.Parse("b2c3d4e5-f6a7-b8c9-d0e1-f2a3b4c5d6a1"), // Jane Smith
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Password = "securepass456",
                    Address = "456 Oak Avenue, Metropolis, USA",
                    IsVerified = false,
                    Image = "jane_smith.jpg",
                    Wishlists = new List<Wishlist>(),
                    Orders = new List<Order>()
                },
                new User
                {
                    UserId = Guid.Parse("c3d4e5f6-a7b8-c9d0-e1f2-a3b4c5d6a1b2"), // Alice Brown
                    Name = "Alice Brown",
                    Email = "alice.brown@example.com",
                    Password = "alicebrown789",
                    Address = "789 Maple Drive, Gotham, USA",
                    IsVerified = true,
                    Image = "alice_brown.jpg",
                    Wishlists = new List<Wishlist>(),
                    Orders = new List<Order>()
                },
                new User
                {
                    UserId = Guid.Parse("d4e5f6a7-b8c9-d0e1-f2a3-b4c5d6a1b2c3"), // Bob Johnson
                    Name = "Bob Johnson",
                    Email = "bob.johnson@example.com",
                    Password = "bobsecurepass",
                    Address = "321 Pine Lane, Star City, USA",
                    IsVerified = false,
                    Image = "bob_johnson.jpg",
                    Wishlists = new List<Wishlist>(),
                    Orders = new List<Order>()
                },
                new User
                {
                    UserId = Guid.Parse("e5f6a7b8-c9d0-e1f2-a3b4-c5d6a1b2c3d4"), // Charlie Davis
                    Name = "Charlie Davis",
                    Email = "charlie.davis@example.com",
                    Password = "charliedavis123",
                    Address = "654 Cedar Road, Coast City, USA",
                    IsVerified = true,
                    Image = "charlie_davis.jpg",
                    Wishlists = new List<Wishlist>(),
                    Orders = new List<Order>()
                }
            );
        }
    }
}
