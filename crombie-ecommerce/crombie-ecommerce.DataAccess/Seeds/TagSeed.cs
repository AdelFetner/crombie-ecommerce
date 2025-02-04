using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class TagSeed : IEntityTypeConfiguration<Tag>
    {
        public static readonly Guid TechTag = Guid.Parse("6a5b4c3d-2e1f-4a8b-9c7d-3e6f5a4b2c1d");
        public static readonly Guid FashionTag = Guid.Parse("7b6c5d4e-3f2a-5b9a-8d7c-4f6e5a3b2c1e");
        public static readonly Guid KitchenTag = Guid.Parse("8c7d6e5f-4a3b-6c0b-9e8d-5f7e6b4c3a2d");
        public static readonly Guid FitnessTag = Guid.Parse("9d8e7f0a-5b4c-7d1c-0a9e-6d8f7c5b3a2e");
        public static readonly Guid ReadingTag = Guid.Parse("0a9b8c7d-6c5d-8e2d-1b0f-7e9a8d6c4b3a");

        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasData(
                new Tag
                {
                    TagId = TechTag,
                    Name = "Tech",
                    Description = "Latest technology products",
                    WishlistId = WishlistSeed.TechWishlist
                },
                new Tag
                {
                    TagId = FashionTag,
                    Name = "Fashion",
                    Description = "Trending fashion items",
                    WishlistId = WishlistSeed.FashionWishlist
                },
                new Tag
                {
                    TagId = KitchenTag,
                    Name = "Kitchen",
                    Description = "Essential kitchen tools",
                    WishlistId = WishlistSeed.KitchenWishlist
                },
                new Tag
                {
                    TagId = FitnessTag,
                    Name = "Fitness",
                    Description = "Workout and exercise gear",
                    WishlistId = WishlistSeed.FitnessWishlist
                },
                new Tag
                {
                    TagId = ReadingTag,
                    Name = "Reading",
                    Description = "Books and literature",
                    WishlistId = WishlistSeed.ReadingWishlist
                }
            );
        }
    }
}