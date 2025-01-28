using crombie_ecommerce.Models;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.DataAccess.Contexts
{
    public class ShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }



        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // builder for user entity
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("User");
                user.HasKey(u => u.UserId);
                user.Property(u => u.Image);
                user.Property(u => u.Name).IsRequired().HasMaxLength(50);
                user.Property(u => u.Email).IsRequired();
                user.Property(u => u.Password).IsRequired().HasMaxLength(100);
                user.Property(u => u.Address).IsRequired(false);
                user.Property(u => u.IsVerified).HasDefaultValue(false);

                // user to orders has a one to many relationship
                user.HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

          

            });

            // builder for product entity
            modelBuilder.Entity<Product>(product =>
            {
                product.ToTable("Product");

                product.HasKey(p => p.ProductId);
                product.Property(p => p.Image);
                product.Property(p => p.ProductId);
                product.Property(p => p.Name).IsRequired().HasMaxLength(50);
                product.Property(p => p.Description).HasMaxLength(100);
                product.Property(p => p.Price).HasColumnType("decimal(18,2)");

                // product to user (doesn't exist anymore)
                /*product.HasOne(p => p.User)
                    .WithOne(u => u.Product)

                    .HasForeignKey<User>(u => u.ProductId)

                    .IsRequired(false);*/

                // product to brand
                product.HasOne(p => p.Brand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                // product to categories, using a many to many relationship (doing a join table named ProductCategory)
                product.HasMany(p => p.Categories)
                    .WithMany(c => c.Products)
                    // join table
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductCategory",
                        // join table relation to category and product
                        j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                        j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                        // makes a CreatedAt for the table
                        j =>
                        {
                            j.Property<DateTime>("CreatedAt").HasDefaultValueSql("CURRENT_TIMESTAMP");
                            j.HasKey("ProductId", "CategoryId");
                        }
                    );

            });

            // builder for wishlist entity
            modelBuilder.Entity<Wishlist>(wishlist =>
            {
                wishlist.ToTable("Wishlist");
                wishlist.HasKey(w => w.WishlistId);
                wishlist.Property(w => w.Name).IsRequired().HasMaxLength(50);

                // user to wishlist has a one to many relationship
                wishlist.HasOne(w => w.User)
                     .WithMany(u => u.Wishlists)
                     .HasForeignKey(w => w.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

                // product to wishlist has a many to many relationship
                wishlist.HasMany(w => w.Products)
                    .WithMany(p => p.Wishlists)
                    // join table for product and wishlist 
                    .UsingEntity<Dictionary<string, object>>(
                        "WishlistProduct",
                        w => w.HasOne<Product>()
                            .WithMany()
                            .HasForeignKey("ProductId")
                            .OnDelete(DeleteBehavior.Cascade),
                        p => p.HasOne<Wishlist>()
                            .WithMany()
                            .HasForeignKey("WishlistId")
                            .OnDelete(DeleteBehavior.Cascade),
                        t =>
                        {
                            t.Property<DateTime>("CreatedAt")
                                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                            t.HasKey("ProductId", "WishlistId");
                        }
                    );

                // wl to tags has a one to many relationship
                wishlist.HasMany(w => w.Tags)
                         .WithOne(t => t.Wishlist)
                         .HasForeignKey(t => t.WishlistId)
                         .OnDelete(DeleteBehavior.Cascade)
                         .IsRequired(false);
            });

            // builder for tags entity 
            modelBuilder.Entity<Tag>(tag =>
            {
                tag.ToTable("Tag");
                tag.HasKey(t => t.TagId);
                tag.Property(t => t.Name).IsRequired().HasMaxLength(50);
                tag.Property(t => t.Description).HasMaxLength(100); 
            });

            // builder for brand entity
            modelBuilder.Entity<Brand>(brand =>
            {
                brand.ToTable("Brand");
                brand.HasKey(b => b.BrandId);
                brand.Property(b => b.BrandId).HasDefaultValueSql("NEWID()");
                brand.Property(b => b.Name).IsRequired().HasMaxLength(100);
                brand.Property(b => b.Description).HasMaxLength(1000);
                brand.Property(b => b.WebsiteUrl).HasMaxLength(255);

                // brand to products has a one to many relationship
                brand.HasMany(b => b.Products)
                    .WithOne(p => p.Brand)
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // builder for category entity
            modelBuilder.Entity<Category>(category =>
            {
                category.ToTable("Category");
                category.HasKey(c => c.CategoryId);
                category.Property(c => c.CategoryId).HasDefaultValueSql("NEWID()");
                category.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                category.Property(c => c.Description)
                    .HasMaxLength(1000);
            });

            //builder for order entity
            modelBuilder.Entity<Order>(order =>
            {   
                order.ToTable("Order");
                order.HasKey(o=>o.OrderId);
                order.Property(o => o.OrderId).HasDefaultValueSql("NEWID()");
                order.Property(o => o.OrderDate).IsRequired();
                order.Property(o=>o.Status).IsRequired();
                order.Property(o => o.TotalAmount).IsRequired();
                order.Property(o => o.ShippingAddress).HasMaxLength(100);
                order.Property(o => o.PaymentMethod).IsRequired();

                //relation user - order (one to many)
                order.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
            });

            //builder for order detail entity
            modelBuilder.Entity<OrderDetail>(orderD => 
            {
                orderD.ToTable("OrderDetail");
                orderD.HasKey(od => od.DetailId);
                orderD.Property(od => od.OrderId).HasDefaultValueSql("NEWID()");
                orderD.Property(od=>od.Quantity).IsRequired();
                orderD.Property(od => od.Price).IsRequired();
                orderD.Property(od => od.Subtotal).IsRequired();

                //relation order - orderDetails (one to many)
                orderD.HasOne(od=>od.Order)
                .WithMany(o=>o.OrderDetails)
                .HasForeignKey(od => od.OrderId);


                //relation orderDetails -  product (many to one)
                orderD.HasOne(od=>od.Product)
                .WithMany(p=>p.OrderDetails)
                .HasForeignKey(od => od.ProductId);
            });

            //builder for notifications entity
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotfId);
                entity.Property(n => n.NotificationType).HasMaxLength(50).IsRequired();
                entity.Property(n => n.Message).HasMaxLength(100).IsRequired();
                entity.Property(n => n.CreatedDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(n => n.IsRead).HasDefaultValue(false);


                // relation notification - product (many to one)
                entity.HasOne(n => n.Product)
                      .WithMany(p => p.Notifications)
                      .HasForeignKey(n => n.ProductId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(); // Ensure the foreign key is required

                // relation notification - wishlist (many to one)
                entity.HasOne(n => n.Wishlist)
                      .WithMany(w => w.Notifications)
                      .HasForeignKey(n => n.WishlistId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);
                entity.Property(c => c.TotalAmount);

                //User - cart relation (one to one)
                entity.HasOne(c => c.User)
                      .WithOne(u => u.Cart)
                      .HasForeignKey<Cart>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // cart - cartItems relation (one to many)
                entity.HasMany(c => c.Items)
                      .WithOne(ci => ci.Cart)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(entity => entity.CartId);


                //producto - cartItem relation (one to many)
                entity.HasOne(ci => ci.Product)
                      .WithMany()
                      .HasForeignKey(ci => ci.ProductId)
                      .OnDelete(DeleteBehavior.Restrict); //to prevent delete a product when want to delete an CartItem


                modelBuilder.Entity<CartItem>()
                            .Property(ci => ci.Total)
                            .HasComputedColumnSql("[Quantity] * [Price]");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}