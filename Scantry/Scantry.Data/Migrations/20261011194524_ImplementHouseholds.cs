using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scantry.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImplementHouseholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    HouseholdId = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the Household entity.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseholdName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name for the Household entity; supports Unicode."),
                    AccessToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "GUID Access Token for joining the Household.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.HouseholdId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the Product entity.")
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "HouseholdUsers",
                columns: table => new
                {
                    HouseholdId = table.Column<int>(type: "int", nullable: false, comment: "Id of the Household."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Id of the User.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdUsers", x => new { x.HouseholdId, x.UserId });
                    table.ForeignKey(
                        name: "FK_HouseholdUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdUsers_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseholdProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "Id of the Product."),
                    HouseholdId = table.Column<int>(type: "int", nullable: false, comment: "Id of the Household.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdProducts", x => new { x.ProductId, x.HouseholdId });
                    table.ForeignKey(
                        name: "FK_HouseholdProducts_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdProducts_HouseholdId",
                table: "HouseholdProducts",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdUsers_UserId",
                table: "HouseholdUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseholdProducts");

            migrationBuilder.DropTable(
                name: "HouseholdUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Households");
        }
    }
}
