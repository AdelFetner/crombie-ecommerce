using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crombie_ecommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HistoryTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoryBrand",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryBrand", x => x.OriginalId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryCategories",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryCategories", x => x.OriginalId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryOrder",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryOrder", x => x.OriginalId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryOrderDetails",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryOrderDetails", x => x.OriginalId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryProduct",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryProduct", x => x.OriginalId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryTags",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryTags", x => x.OriginalId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryUser",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryUser", x => x.OriginalId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryWishlist",
                columns: table => new
                {
                    OriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryWishlist", x => x.OriginalId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryBrand");

            migrationBuilder.DropTable(
                name: "HistoryCategories");

            migrationBuilder.DropTable(
                name: "HistoryOrder");

            migrationBuilder.DropTable(
                name: "HistoryOrderDetails");

            migrationBuilder.DropTable(
                name: "HistoryProduct");

            migrationBuilder.DropTable(
                name: "HistoryTags");

            migrationBuilder.DropTable(
                name: "HistoryUser");

            migrationBuilder.DropTable(
                name: "HistoryWishlist");
        }
    }
}
