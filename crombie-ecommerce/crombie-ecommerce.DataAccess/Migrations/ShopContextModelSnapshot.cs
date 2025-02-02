﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using crombie_ecommerce.DataAccess.Contexts;

#nullable disable

namespace crombie_ecommerce.DataAccess.Migrations
{
    [DbContext(typeof(ShopContext))]
    partial class ShopContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProductCategory", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("ProductId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ProductCategory", (string)null);
                });

            modelBuilder.Entity("WishlistProduct", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WishlistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("ProductId", "WishlistId");

                    b.HasIndex("WishlistId");

                    b.ToTable("WishlistProduct", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Brand", b =>
                {
                    b.Property<Guid>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WebsiteUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("BrandId");

                    b.ToTable("Brand", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoryId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryBrand", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryBrand", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryCategory", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryCategories", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryOrder", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryOrder", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryOrderDetails", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryOrderDetails", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryProduct", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryProduct", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryTag", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryTags", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryUser", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryUser", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.HistoryWishlist", b =>
                {
                    b.Property<Guid>("OriginalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EntityJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OriginalId");

                    b.ToTable("HistoryWishlist", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Notification", b =>
                {
                    b.Property<Guid>("NotfId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NotificationType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WishlistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("NotfId");

                    b.HasIndex("ProductId");

                    b.HasIndex("WishlistId");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.OrderDetail", b =>
                {
                    b.Property<Guid>("DetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("Subtotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("DetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetail", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductId");

                    b.HasIndex("BrandId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("WishlistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TagId");

                    b.HasIndex("WishlistId");

                    b.ToTable("Tag", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVerified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Wishlist", b =>
                {
                    b.Property<Guid>("WishlistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("NotfId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TagsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("WishlistId");

                    b.HasIndex("UserId");

                    b.ToTable("Wishlist", (string)null);
                });

            modelBuilder.Entity("ProductCategory", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("crombie_ecommerce.Models.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WishlistProduct", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("crombie_ecommerce.Models.Entities.Wishlist", null)
                        .WithMany()
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Notification", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.Product", "Product")
                        .WithMany("Notifications")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("crombie_ecommerce.Models.Entities.Wishlist", "Wishlist")
                        .WithMany("Notifications")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Order", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.OrderDetail", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId");

                    b.HasOne("crombie_ecommerce.Models.Entities.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Product", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Tag", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.Wishlist", "Wishlist")
                        .WithMany("Tags")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Wishlist", b =>
                {
                    b.HasOne("crombie_ecommerce.Models.Entities.User", "User")
                        .WithMany("Wishlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Product", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("crombie_ecommerce.Models.Entities.Wishlist", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
