using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crombie_ecommerce.DataAccess.Seeds
{
    public class WishlistSeed : IEntityTypeConfiguration<Wishlist>
    {
        // Random GUIDs
        public static readonly Guid TechWishlist = Guid.Parse("9a8b7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d");
        public static readonly Guid FashionWishlist = Guid.Parse("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e");
        public static readonly Guid KitchenWishlist = Guid.Parse("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f");
        public static readonly Guid FitnessWishlist = Guid.Parse("3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a");
        public static readonly Guid ReadingWishlist = Guid.Parse("4e5f6a7b-8c9d-0e1f-2a3b-4c5d6e7f8a9b");

        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasData(
                new Wishlist
                {
                    WishlistId = TechWishlist,
                    Name = "Tech Wishlist",
                    Description = "Latest gadgets I want",
                    UserId = UserSeed.AlexJohnson // Links to Alex Johnson
                },
                new Wishlist
                {
                    WishlistId = FashionWishlist,
                    Name = "Fashion Picks",
                    Description = "Seasonal outfits",
                    UserId = UserSeed.MariaGomez
                },
                new Wishlist
                {
                    WishlistId = KitchenWishlist,
                    Name = "Kitchen Upgrades",
                    Description = "Cooking essentials",
                    UserId = UserSeed.JohnDoe
                },
                new Wishlist
                {
                    WishlistId = FitnessWishlist,
                    Name = "Fitness Gear",
                    Description = "Workout equipment",
                    UserId = UserSeed.EmmaWilson
                },
                new Wishlist
                {
                    WishlistId = ReadingWishlist,
                    Name = "Book Wishlist",
                    Description = "2024 reading list",
                    UserId = UserSeed.LiamBrown
                }
            );

            // Link to Products (M2M)
            builder.HasMany(w => w.Products)
                .WithMany(p => p.Wishlists)
                .UsingEntity<Dictionary<string, object>>(
                    "WishlistProduct",
                    j => j.HasData(
                        new { WishlistId = TechWishlist, ProductId = ProductSeed.SmartphoneX },
                        new { WishlistId = FashionWishlist, ProductId = ProductSeed.DenimJacket },
                        new { WishlistId = KitchenWishlist, ProductId = ProductSeed.BlenderPro },
                        new { WishlistId = FitnessWishlist, ProductId = ProductSeed.YogaMat },
                        new { WishlistId = ReadingWishlist, ProductId = ProductSeed.MysteryNovel }
                    )
                );
        }
    }
}