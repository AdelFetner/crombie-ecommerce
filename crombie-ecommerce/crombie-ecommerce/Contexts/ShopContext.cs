﻿using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Contexts
{
    public class ShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Tags> Tags { get; set; }

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
                    .HasForeignKey<User>(u => u.ProductId)
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
                product.Property(p => p.Brand).HasMaxLength(50);
                product.Property(p => p.Category).HasMaxLength(50);

                // product to user
                product.HasOne(p => p.User)
                    .WithOne(u => u.Product)
                    .HasForeignKey<User>(u => u.UserId)
                    .IsRequired(false);

                // product to wl
                product.HasOne(p => p.Wishlist)
                    .WithOne(w => w.Product)
                    .HasForeignKey<Wishlist>(w => w.ProductId)
                    .IsRequired(false);
            });

            // builder for wishlist entity
            modelBuilder.Entity<Wishlist>(wishlist =>
            {
                wishlist.ToTable("Wishlist");
                wishlist.HasKey(w => w.WishlistId);
                wishlist.Property(w => w.Name).IsRequired().HasMaxLength(50);

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

                // wl to tags has a many to many relationship
                wishlist.HasMany(w => w.Tags)
                     .WithOne(t => t.Wishlist)
                     .HasForeignKey(t => t.WishlistId)
                     .OnDelete(DeleteBehavior.Cascade)
                     .IsRequired(false);
            });

            // builder for tags entity 
            modelBuilder.Entity<Tags>(tag =>
            {
                tag.ToTable("Tags");
                tag.HasKey(t => t.TagId);
                tag.Property(t => t.Name).IsRequired().HasMaxLength(50);
                tag.Property(t => t.Description).HasMaxLength(100); 
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}