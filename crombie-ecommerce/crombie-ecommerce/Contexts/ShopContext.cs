using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Contexts
{
    public class ShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // builder for user entity
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("User");
                user.HasKey(u => u.UserId);
                user.Property(u => u.Name).IsRequired().HasMaxLength(50);
                user.Property(u => u.Email).IsRequired();
                user.Property(u => u.Password).IsRequired().HasMaxLength(100);
                user.Property(u => u.IsVerified).HasDefaultValue(false);

                // user to wl has a one to one relationship
                user.HasOne(u => u.Wishlist)
                    .WithOne(w => w.User)
                    .HasForeignKey<Wishlist>(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                user.HasOne(u => u.Product)
                    .WithOne(p => p.User)
                    .HasForeignKey<Product>(u => u.UserId)
                    .IsRequired(false);

               

            });

            // builder for product entity
            modelBuilder.Entity<Product>(product =>
            {
                product.ToTable("Product");

                product.HasKey(p => p.Id);
                product.Property(p => p.Id).HasDefaultValueSql("NEWID()");
                product.Property(p => p.Name).IsRequired().HasMaxLength(50);
                product.Property(p => p.Description).HasMaxLength(100);
                product.Property(p => p.Price).HasColumnType("decimal(18,2)");
                product.Property(p => p.Category).HasMaxLength(50);

                // product to user
                product.HasOne(p => p.User)
                    .WithOne(u => u.Product)

                    .HasForeignKey<User>(u => u.ProductId)

                    .IsRequired(false);

                // product to wl
                product.HasOne(p => p.Wishlist)
                    .WithOne(w => w.Product)
                    .HasForeignKey<Wishlist>(w => w.ProductId)
                    .IsRequired(false);

                // product to brand
                product.HasOne(p => p.Brand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            // builder for wishlist entity
            modelBuilder.Entity<Wishlist>(wishlist =>
            {
                wishlist.ToTable("Wishlist");
                wishlist.HasKey(w => w.WishlistId);
                wishlist.Property(w => w.Name).IsRequired().HasMaxLength(50);
                wishlist.Property(w => w.Tag).IsRequired().HasMaxLength(100);

                // wl to user has a one to one relationship
                wishlist.HasOne(w => w.User)
                    .WithOne(u => u.Wishlist)
                    .HasForeignKey<Wishlist>(w => w.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);

                // wl to product has a  one to one relationship
                wishlist.HasOne(w => w.Product)
                    .WithOne(p => p.Wishlist)
                    .HasForeignKey<Wishlist>(w => w.ProductId)
                    .IsRequired(false);

            });

            // builder for brand entity
            modelBuilder.Entity<Brand>(brand =>
            {
                brand.ToTable("Brand");
                brand.HasKey(b => b.BrandId);
                brand.Property(b => b.Name).IsRequired().HasMaxLength(100);
                brand.Property(b => b.Description).HasMaxLength(1000);
                brand.Property(b => b.WebsiteUrl).HasMaxLength(255);

                // brand to products (one-to-many relationship)
                brand.HasMany(b => b.Products)
                    .WithOne(p => p.Brand)
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}