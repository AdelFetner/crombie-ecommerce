using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace crombie_ecommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class featSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.WishlistId);
                    table.ForeignKey(
                        name: "FK_Wishlist_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    DetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "[Quantity] * [Price]"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_Wishlist_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlist",
                        principalColumn: "WishlistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tag_Wishlist_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlist",
                        principalColumn: "WishlistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishlistProduct",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistProduct", x => new { x.ProductId, x.WishlistId });
                    table.ForeignKey(
                        name: "FK_WishlistProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistProduct_Wishlist_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlist",
                        principalColumn: "WishlistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "BrandId", "Description", "Name", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("a3b2c1d0-1234-5678-9abc-def012345678"), "Consumer electronics innovator", "ElectroTech", "https://electrotech.com" },
                    { new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), "Contemporary street fashion", "UrbanWear", "https://urbanwear.style" },
                    { new Guid("c5d4e3f2-3456-7890-1abc-def23456789a"), "Premium homeware solutions", "HomeEssentials", "https://homeessentials.co" },
                    { new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), "High-performance athletic gear", "SportFlex", "https://sportflex.com" },
                    { new Guid("e7f6f5e4-5678-9012-3abc-def456789abc"), "Literary classics & new releases", "BookHaven", "https://bookhaven.store" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), "Physical and digital media", "Books & Media" },
                    { new Guid("c5d4e3f2-3456-7890-1abc-def23456789a"), "Athletic gear and outdoor equipment", "Sports & Outdoors" },
                    { new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), "Furniture and home decor", "Home & Living" },
                    { new Guid("e7f6a5b4-5678-9012-3abc-def456789abc"), "Clothing and fashion items", "Apparel" },
                    { new Guid("f8a7b6c5-6789-0123-4abc-def56789abcd"), "Devices, gadgets, and tech accessories", "Electronics" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Address", "Email", "Image", "IsVerified", "Name", "Password" },
                values: new object[,]
                {
                    { new Guid("3d4a7c9f-2e1b-4a8d-9c3f-6b2e1d0a4c7b"), "La casa de los olmedo", "alex.j@example.com", "user-alex.jpg", true, "Alex Johnson", "SecurePass123!" },
                    { new Guid("5b3d9f1a-7e2c-48d9-9a1b-4f6c3e2d0a5b"), "321 Elm St, Bookville", "liam.b@example.com", "user-liam.jpg", true, "Liam Brown", "L1amBrown!" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Address", "Email", "Image", "Name", "Password" },
                values: new object[,]
                {
                    { new Guid("8c2d1f9a-4b3e-4567-8910-3f6e5d4c2b1a"), "Palito Santo 2040", "emma.w@example.com", "user-emma.jpg", "Emma Wilson", "Emm@2024!" },
                    { new Guid("a1b8f45c-9d32-4e67-82f1-0c3d5e7f9a2b"), "123 Main St, TechCity", "maria.g@example.com", "user-maria.jpg", "Maria Gomez", "MariaG0mez!" },
                    { new Guid("e6f9d287-5c34-4a1b-89d0-3b7a2c4e5f1a"), "456 Oak St, MetroCity", "john.d@example.com", "user-john.jpg", "John Doe", "DoeJ0hn!" }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "OrderId", "OrderDate", "PaymentMethod", "ShippingAddress", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { new Guid("0c1d2e3f-4a5b-6c7d-8e9f-0a1b2c3d4e5f"), new DateTime(2025, 2, 2, 0, 13, 34, 269, DateTimeKind.Utc).AddTicks(9015), "Google Pay", "321 Elm St, Bookville", "Canceled", 24.99m, new Guid("5b3d9f1a-7e2c-48d9-9a1b-4f6c3e2d0a5b") },
                    { new Guid("6d5e4f3a-2b1c-0d9e-8f7a-6b5c4d3e2f1a"), new DateTime(2025, 1, 26, 2, 13, 34, 269, DateTimeKind.Utc).AddTicks(8998), "Credit Card", "123 Main St, TechCity", "Delivered", 999.99m, new Guid("3d4a7c9f-2e1b-4a8d-9c3f-6b2e1d0a4c7b") },
                    { new Guid("7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b"), new DateTime(2025, 1, 30, 2, 13, 34, 269, DateTimeKind.Utc).AddTicks(9009), "PayPal", "456 Oak St, MetroCity", "Processing", 129.95m, new Guid("a1b8f45c-9d32-4e67-82f1-0c3d5e7f9a2b") },
                    { new Guid("8a9b0c1d-2e3f-4a5b-6c7d-8e9f0a1b2c3d"), new DateTime(2025, 2, 1, 2, 13, 34, 269, DateTimeKind.Utc).AddTicks(9011), "Debit Card", "789 Pine Rd, VillageTown", "Shipped", 89.99m, new Guid("e6f9d287-5c34-4a1b-89d0-3b7a2c4e5f1a") },
                    { new Guid("9b0c1d2e-3f4a-5b6c-7d8e-9f0a1b2c3d4e"), new DateTime(2025, 2, 1, 14, 13, 34, 269, DateTimeKind.Utc).AddTicks(9013), "Apple Pay", "La bombonera", "Pending", 69.00m, new Guid("8c2d1f9a-4b3e-4567-8910-3f6e5d4c2b1a") }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductId", "BrandId", "Description", "Image", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), new Guid("e7f6f5e4-5678-9012-3abc-def456789abc"), "Hardcover bestseller novel", "midnight-library.jpg", "Midnight Library: Special Edition", 24.99m },
                    { new Guid("c5d4e3f2-3456-7890-1abc-def23456789a"), new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), "Non-slip 6mm thick mat", "yoga-mat-eco.jpg", "SportFlex Eco Yoga Mat", 34.50m },
                    { new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), new Guid("c5d4e3f2-3456-7890-1abc-def23456789a"), "1500W 8-Speed Countertop Blender", "blender-pro.jpg", "Essentials Blender Pro", 89.99m },
                    { new Guid("e7f6a5b4-5678-9012-3abc-def456789abc"), new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), "Slim-fit washed denim", "denim-jacket-urban.jpg", "Urban Classic Denim Jacket", 129.95m },
                    { new Guid("f8a7b6c5-6789-0123-4abc-def56789abcd"), new Guid("a3b2c1d0-1234-5678-9abc-def012345678"), "6.8\" AMOLED, 256GB Storage", "smartphone-x10.jpg", "ElectroTech X10 Pro", 999.99m }
                });

            migrationBuilder.InsertData(
                table: "Wishlist",
                columns: new[] { "WishlistId", "Description", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e"), "Seasonal outfits", "Fashion Picks", new Guid("a1b8f45c-9d32-4e67-82f1-0c3d5e7f9a2b") },
                    { new Guid("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f"), "Cooking essentials", "Kitchen Upgrades", new Guid("e6f9d287-5c34-4a1b-89d0-3b7a2c4e5f1a") },
                    { new Guid("3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a"), "Workout equipment", "Fitness Gear", new Guid("8c2d1f9a-4b3e-4567-8910-3f6e5d4c2b1a") },
                    { new Guid("4e5f6a7b-8c9d-0e1f-2a3b-4c5d6e7f8a9b"), "2024 reading list", "Book Wishlist", new Guid("5b3d9f1a-7e2c-48d9-9a1b-4f6c3e2d0a5b") },
                    { new Guid("9a8b7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d"), "Latest gadgets I want", "Tech Wishlist", new Guid("3d4a7c9f-2e1b-4a8d-9c3f-6b2e1d0a4c7b") }
                });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "NotificationId", "CreatedDate", "Message", "NotificationType", "ProductId", "WishlistId" },
                values: new object[] { new Guid("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e"), new DateTime(2025, 1, 31, 2, 13, 34, 269, DateTimeKind.Utc).AddTicks(9364), "Price dropped on ElectroTech X10 Pro!", "Price Alert", new Guid("f8a7b6c5-6789-0123-4abc-def56789abcd"), new Guid("9a8b7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d") });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "NotificationId", "CreatedDate", "IsRead", "Message", "NotificationType", "ProductId", "WishlistId" },
                values: new object[] { new Guid("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f"), new DateTime(2025, 2, 1, 14, 13, 34, 269, DateTimeKind.Utc).AddTicks(9366), true, "Urban Denim Jacket back in stock", "Restock", new Guid("e7f6a5b4-5678-9012-3abc-def456789abc"), new Guid("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e") });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "NotificationId", "CreatedDate", "Message", "NotificationType", "ProductId", "WishlistId" },
                values: new object[,]
                {
                    { new Guid("3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a"), new DateTime(2025, 2, 1, 2, 13, 34, 269, DateTimeKind.Utc).AddTicks(9368), "New item added to Kitchen Upgrades", "Wishlist Update", new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), new Guid("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("5f6a7b8c-9d0e-1f2a-3b4c-5d6e7f8a9b0c"), new DateTime(2025, 2, 2, 1, 13, 34, 269, DateTimeKind.Utc).AddTicks(9369), "20% off all books this week!", "Special Offer", new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), new Guid("4e5f6a7b-8c9d-0e1f-2a3b-4c5d6e7f8a9b") }
                });

            migrationBuilder.InsertData(
                table: "OrderDetail",
                columns: new[] { "DetailId", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("1d2e3f4a-5b6c-7d8e-9f0a-1b2c3d4e5f6a"), new Guid("6d5e4f3a-2b1c-0d9e-8f7a-6b5c4d3e2f1a"), 999.99m, new Guid("f8a7b6c5-6789-0123-4abc-def56789abcd"), 1 },
                    { new Guid("2e3f4a5b-6c7d-8e9f-0a1b-2c3d4e5f6a7b"), new Guid("7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b"), 129.95m, new Guid("e7f6a5b4-5678-9012-3abc-def456789abc"), 1 },
                    { new Guid("3f4a5b6c-7d8e-9f0a-1b2c-3d4e5f6a7b8c"), new Guid("8a9b0c1d-2e3f-4a5b-6c7d-8e9f0a1b2c3d"), 89.99m, new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), 1 },
                    { new Guid("4a5b6c7d-8e9f-0a1b-2c3d-4e5f6a7b8c9d"), new Guid("9b0c1d2e-3f4a-5b6c-7d8e-9f0a1b2c3d4e"), 34.50m, new Guid("c5d4e3f2-3456-7890-1abc-def23456789a"), 2 },
                    { new Guid("5b6c7d8e-9f0a-1b2c-3d4e-5f6a7b8c9d0e"), new Guid("0c1d2e3f-4a5b-6c7d-8e9f-0a1b2c3d4e5f"), 24.99m, new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), 1 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), new Guid("b4c3d2e1-2345-6789-0abc-def123456789") },
                    { new Guid("c5d4e3f2-3456-7890-1abc-def23456789a"), new Guid("c5d4e3f2-3456-7890-1abc-def23456789a") },
                    { new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab") },
                    { new Guid("e7f6a5b4-5678-9012-3abc-def456789abc"), new Guid("e7f6a5b4-5678-9012-3abc-def456789abc") },
                    { new Guid("f8a7b6c5-6789-0123-4abc-def56789abcd"), new Guid("f8a7b6c5-6789-0123-4abc-def56789abcd") }
                });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "TagId", "Description", "Name", "WishlistId" },
                values: new object[,]
                {
                    { new Guid("0a9b8c7d-6c5d-8e2d-1b0f-7e9a8d6c4b3a"), "Books and literature", "Reading", new Guid("4e5f6a7b-8c9d-0e1f-2a3b-4c5d6e7f8a9b") },
                    { new Guid("6a5b4c3d-2e1f-4a8b-9c7d-3e6f5a4b2c1d"), "Latest technology products", "Tech", new Guid("9a8b7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d") },
                    { new Guid("7b6c5d4e-3f2a-5b9a-8d7c-4f6e5a3b2c1e"), "Trending fashion items", "Fashion", new Guid("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("8c7d6e5f-4a3b-6c0b-9e8d-5f7e6b4c3a2d"), "Essential kitchen tools", "Kitchen", new Guid("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("9d8e7f0a-5b4c-7d1c-0a9e-6d8f7c5b3a2e"), "Workout and exercise gear", "Fitness", new Guid("3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a") }
                });

            migrationBuilder.InsertData(
                table: "WishlistProduct",
                columns: new[] { "ProductId", "WishlistId" },
                values: new object[,]
                {
                    { new Guid("b4c3d2e1-2345-6789-0abc-def123456789"), new Guid("4e5f6a7b-8c9d-0e1f-2a3b-4c5d6e7f8a9b") },
                    { new Guid("c5d4e3f2-3456-7890-1abc-def23456789a"), new Guid("3d4e5f6a-7b8c-9d0e-1f2a-3b4c5d6e7f8a") },
                    { new Guid("d6e5f4d3-4567-8901-2abc-def3456789ab"), new Guid("2c3d4e5f-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("e7f6a5b4-5678-9012-3abc-def456789abc"), new Guid("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("f8a7b6c5-6789-0123-4abc-def56789abcd"), new Guid("9a8b7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ProductId",
                table: "Notification",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_WishlistId",
                table: "Notification",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandId",
                table: "Product",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_CategoryId",
                table: "ProductCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_WishlistId",
                table: "Tag",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistProduct_WishlistId",
                table: "WishlistProduct",
                column: "WishlistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "WishlistProduct");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
