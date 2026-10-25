using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scantry.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImplementProductsandHouseholdProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseholdProducts_Products_ProductId",
                table: "HouseholdProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HouseholdProducts",
                table: "HouseholdProducts");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Product barcode.");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Product brand.");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Products",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                comment: "Product model.");

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "Products",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: false,
                defaultValue: "",
                comment: "Product type.");

            migrationBuilder.AddColumn<string>(
                name: "SizeUnit",
                table: "Products",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                comment: "Product measuring unit.");

            migrationBuilder.AddColumn<decimal>(
                name: "SizeValue",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Product size value.");

            migrationBuilder.AddColumn<string>(
                name: "Variant",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "Product variation (if applicable).");

            migrationBuilder.AddColumn<int>(
                name: "HouseholdProductId",
                table: "HouseholdProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Primary key for the HouseholdProduct entity.")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "HouseholdProducts",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                comment: "Category of the product - user-defined.");

            migrationBuilder.AddColumn<int>(
                name: "CurrentQuantity",
                table: "HouseholdProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Current quantity of the product in the household.");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpirationDate",
                table: "HouseholdProducts",
                type: "date",
                nullable: true,
                comment: "Expiration date of the product in the household - if applicable.");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "HouseholdProducts",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Flag for favourite products.");

            migrationBuilder.AddColumn<int>(
                name: "MinimumThreshold",
                table: "HouseholdProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Threshold for buying reminder - user-defined.");

            migrationBuilder.AddColumn<string>(
                name: "ScannedByUserId",
                table: "HouseholdProducts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ScannedOn",
                table: "HouseholdProducts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                comment: "Date of scanning the product.");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HouseholdProducts",
                table: "HouseholdProducts",
                column: "HouseholdProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Barcode",
                table: "Products",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdProducts_Category",
                table: "HouseholdProducts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdProducts_ProductId",
                table: "HouseholdProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdProducts_ScannedByUserId",
                table: "HouseholdProducts",
                column: "ScannedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseholdProducts_AspNetUsers_ScannedByUserId",
                table: "HouseholdProducts",
                column: "ScannedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HouseholdProducts_Products_ProductId",
                table: "HouseholdProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseholdProducts_AspNetUsers_ScannedByUserId",
                table: "HouseholdProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_HouseholdProducts_Products_ProductId",
                table: "HouseholdProducts");

            migrationBuilder.DropIndex(
                name: "IX_Products_Barcode",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HouseholdProducts",
                table: "HouseholdProducts");

            migrationBuilder.DropIndex(
                name: "IX_HouseholdProducts_Category",
                table: "HouseholdProducts");

            migrationBuilder.DropIndex(
                name: "IX_HouseholdProducts_ProductId",
                table: "HouseholdProducts");

            migrationBuilder.DropIndex(
                name: "IX_HouseholdProducts_ScannedByUserId",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeValue",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Variant",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HouseholdProductId",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "CurrentQuantity",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "MinimumThreshold",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "ScannedByUserId",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "ScannedOn",
                table: "HouseholdProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HouseholdProducts",
                table: "HouseholdProducts",
                columns: new[] { "ProductId", "HouseholdId" });

            migrationBuilder.AddForeignKey(
                name: "FK_HouseholdProducts_Products_ProductId",
                table: "HouseholdProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
