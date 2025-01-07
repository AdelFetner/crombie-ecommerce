using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crombie_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class fixEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Product_UserId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Product_ProductId",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_User_UserId",
                table: "Wishlist");

            migrationBuilder.DropIndex(
                name: "IX_Wishlist_ProductId",
                table: "Wishlist");

            migrationBuilder.DropIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProductId",
                table: "User",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Product_ProductId",
                table: "User",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Product_WishlistId",
                table: "Wishlist",
                column: "WishlistId",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_User_WishlistId",
                table: "Wishlist",
                column: "WishlistId",
                principalTable: "User",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Product_ProductId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Product_WishlistId",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_User_WishlistId",
                table: "Wishlist");

            migrationBuilder.DropIndex(
                name: "IX_User_ProductId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_ProductId",
                table: "Wishlist",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Product_UserId",
                table: "User",
                column: "UserId",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Product_ProductId",
                table: "Wishlist",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_User_UserId",
                table: "Wishlist",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
