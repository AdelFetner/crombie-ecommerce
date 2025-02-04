using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace crombie_ecommerce.DataAccess.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        // GUIDs follow sequential pattern
        public static readonly Guid AlexJohnson = Guid.Parse("3d4a7c9f-2e1b-4a8d-9c3f-6b2e1d0a4c7b");
        public static readonly Guid MariaGomez = Guid.Parse("a1b8f45c-9d32-4e67-82f1-0c3d5e7f9a2b");
        public static readonly Guid JohnDoe = Guid.Parse("e6f9d287-5c34-4a1b-89d0-3b7a2c4e5f1a");
        public static readonly Guid EmmaWilson = Guid.Parse("8c2d1f9a-4b3e-4567-8910-3f6e5d4c2b1a");
        public static readonly Guid LiamBrown = Guid.Parse("5b3d9f1a-7e2c-48d9-9a1b-4f6c3e2d0a5b");

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    UserId = AlexJohnson,
                    Name = "Alex Johnson",
                    Email = "alex.j@example.com",
                    Password = "SecurePass123!",
                    Address = "La casa de los olmedo",
                    Image = "user-alex.jpg",
                    IsVerified = true
                },
                new User
                {
                    UserId = MariaGomez,
                    Name = "Maria Gomez",
                    Email = "maria.g@example.com",
                    Password = "MariaG0mez!",
                    Address = "123 Main St, TechCity",
                    Image = "user-maria.jpg",
                    IsVerified = false
                },
                new User
                {
                    UserId = JohnDoe,
                    Name = "John Doe",
                    Email = "john.d@example.com",
                    Password = "DoeJ0hn!",
                    Address = "456 Oak St, MetroCity",
                    Image = "user-john.jpg",
                    IsVerified = false
                },
                new User
                {
                    UserId = EmmaWilson,
                    Name = "Emma Wilson",
                    Email = "emma.w@example.com",
                    Password = "Emm@2024!",
                    Address = "Palito Santo 2040",
                    Image = "user-emma.jpg",
                    IsVerified = false
                },
                new User
                {
                    UserId = LiamBrown,
                    Name = "Liam Brown",
                    Email = "liam.b@example.com",
                    Password = "L1amBrown!",
                    Address = "321 Elm St, Bookville",
                    Image = "user-liam.jpg",
                    IsVerified = true
                }
            );
        }
    }
}