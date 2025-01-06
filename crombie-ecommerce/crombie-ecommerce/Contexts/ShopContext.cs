﻿using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Contexts
{
    public class ShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

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
<<<<<<< HEAD
                

                user.HasOne(u => u.Product)
                    .WithOne(p => p.User)
                    .HasForeignKey<Product>(u => u.UserId)
                    .IsRequired(false);

                
=======

                user.HasOne(u => u.Product)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(u => u.ProductId)
                    .IsRequired(false);
>>>>>>> 141dfa9bf68ed9a9243d0405caf993792e57ee00
            });

            // builder for product entity
            modelBuilder.Entity<Product>(product =>
            {
                product.ToTable("Product");
<<<<<<< HEAD
                product.HasKey(p => p.ProductId);
=======
                product.HasKey(p => p.Id);
                product.Property(p => p.Id).HasDefaultValueSql("NEWID()");
>>>>>>> 141dfa9bf68ed9a9243d0405caf993792e57ee00
                product.Property(p => p.Name).IsRequired().HasMaxLength(50);
                product.Property(p => p.Description).HasMaxLength(100); 
                product.Property(p => p.Price).HasColumnType("decimal(18,2)");
                product.Property(p => p.Brand).HasMaxLength(50);
                product.Property(p => p.Category).HasMaxLength(50);

                // product to user
                product.HasOne(p => p.User)
                    .WithOne(u => u.Product)
<<<<<<< HEAD
                    .HasForeignKey<User>(u => u.ProductId)
=======
                    .HasForeignKey<User>(u => u.UserId)
>>>>>>> 141dfa9bf68ed9a9243d0405caf993792e57ee00
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
                wishlist.Property(w => w.Tag).IsRequired().HasMaxLength(100);

                // wl to user has a one to one relationship
                wishlist.HasOne(w => w.User)
                        .WithOne(u => u.Wishlist)
<<<<<<< HEAD
                        .HasForeignKey<Wishlist>(w => w.WishlistId)
=======
                        .HasForeignKey<Wishlist>(w => w.UserId)
>>>>>>> 141dfa9bf68ed9a9243d0405caf993792e57ee00
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired(false);

                // wl to product has a  one to one relationship
                wishlist.HasOne(w => w.Product)
                        .WithOne(p => p.Wishlist)
<<<<<<< HEAD
                        .HasForeignKey<Wishlist>(w => w.WishlistId)
=======
                        .HasForeignKey<Wishlist>(w => w.ProductId)
>>>>>>> 141dfa9bf68ed9a9243d0405caf993792e57ee00
                        .IsRequired(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}