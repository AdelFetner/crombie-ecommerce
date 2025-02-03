using crombie_ecommerce.DataAccess.Seeds;
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
        public DbSet<Stock> Stock { get; set; }

        public DbSet<HistoryWishlist> HistoryWishlists { get; set; }
        public DbSet<HistoryUser> HistoryUsers { get; set; }
        public DbSet<HistoryProduct> HistoryProducts { get; set; }
        public DbSet<HistoryBrand> HistoryBrands { get; set; }
        public DbSet<HistoryOrder> HistoryOrders { get; set; }
        public DbSet<HistoryOrderDetails> HistoryOrderDetails { get; set; }
        public DbSet<HistoryTag> HistoryTags { get; set; }
        public DbSet<HistoryCategory> HistoryCategories { get; set; }
        
        public DbSet<Role> Roles { get; set; }



        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // builder for user entity
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("User");
                user.HasKey(u => u.UserId);
                user.Property(u => u.Image).IsRequired(false);
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

                //user to role relations (one to one)
                user.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();
                    

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
                // product to stock
                product.HasOne(p => p.Stock)
                       .WithOne(s => s.Product)
                       .HasForeignKey<Stock>(s => s.ProductId)
                       .OnDelete(DeleteBehavior.Cascade);
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

                tag.HasOne(t => t.Wishlist)
                        .WithMany(w => w.Tags)
                        .HasForeignKey(t => t.WishlistId)
                        .OnDelete(DeleteBehavior.Cascade);
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
                order.HasKey(o => o.OrderId);
                order.Property(o => o.OrderId).HasDefaultValueSql("NEWID()");
                order.Property(o => o.OrderDate).IsRequired();
                order.Property(o=>o.Status).IsRequired();
                order.Property(o => o.TotalAmount).HasPrecision(18, 2).IsRequired();
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
                orderD.Property(od => od.Quantity).IsRequired();
                orderD.Property(od => od.Price).HasPrecision(18, 2).IsRequired();
                orderD.Property(od => od.Subtotal).HasPrecision(18, 2).IsRequired().HasComputedColumnSql("[Quantity] * [Price]").ValueGeneratedOnAddOrUpdate();
                //relation order - orderDetails (one to many)
                orderD.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


                //relation orderDetails -  product (many to one)
                orderD.HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);
            });

            //builder for notifications entity
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotificationId);
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

            //builder for stock entity
            modelBuilder.Entity<Stock>(stock =>
            {
                stock.ToTable("Stock");
                stock.HasKey(s => s.StockId);
                stock.Property(s => s.Quantity).IsRequired();
                stock.Property(s => s.LastUpdated).IsRequired();

                // relation stock - product (one to one)
                stock.HasOne(s => s.Product)
                    .WithOne(p => p.Stock)
                    .HasForeignKey<Stock>(s => s.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            //builder for history wishlist entity
            modelBuilder.Entity<HistoryWishlist>(entity =>
            {
                entity.ToTable("HistoryWishlist");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });

            //builder for history user entity
            modelBuilder.Entity<HistoryUser>(entity =>
            {
                entity.ToTable("HistoryUser");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });

            //builder for history product entity
            modelBuilder.Entity<HistoryProduct>(entity =>
            {
                entity.ToTable("HistoryProduct");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });

            //builder for history brand entity
            modelBuilder.Entity<HistoryBrand>(entity =>
            {
                entity.ToTable("HistoryBrand");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });

            //builder for history order entity
            modelBuilder.Entity<HistoryOrder>(entity =>
            {
                entity.ToTable("HistoryOrder");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });

            //builder for history order details entity
            modelBuilder.Entity<HistoryOrderDetails>(entity =>
            {
                entity.ToTable("HistoryOrderDetails");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });

            //builder for history order details entity
            modelBuilder.Entity<HistoryCategory>(entity =>
            {
                entity.ToTable("HistoryCategories");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });
            
            //builder for role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r=>r.RoleId);
                entity.Property(e => e.RoleId).ValueGeneratedNever();
                entity.Property(r => r.Name).IsRequired();
                entity.Property(r => r.Description).HasMaxLength(500);
                entity.HasData(
                        new Role { RoleId = 1, Name = "User", Description = "Default user role" },
                        new Role { RoleId = 2, Name = "Admin", Description = "Administrator role" }
                );

            });

            //builder for history order details entity
            modelBuilder.Entity<HistoryTag>(entity =>
            {
                entity.ToTable("HistoryTags");
                entity.HasKey(e => e.OriginalId);
                entity.Property(e => e.EntityJson).IsRequired();
                entity.Property(e => e.ProcessedBy).IsRequired();
                entity.Property(e => e.ProcessedAt).IsRequired();
            });

            modelBuilder.ApplyConfiguration(new BrandSeed());
            modelBuilder.ApplyConfiguration(new CategorySeed());
            modelBuilder.ApplyConfiguration(new ProductSeed());
            modelBuilder.ApplyConfiguration(new UserSeed());
            modelBuilder.ApplyConfiguration(new WishlistSeed());
            modelBuilder.ApplyConfiguration(new OrderSeed());
            modelBuilder.ApplyConfiguration(new OrderDetailSeed());
            modelBuilder.ApplyConfiguration(new TagSeed());
            modelBuilder.ApplyConfiguration(new NotificationSeed());


            base.OnModelCreating(modelBuilder);
        }
    }
}