using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crombie_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class FixUserWishlistRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist",
                column: "UserId",
                unique: true);
        }
    }
}
