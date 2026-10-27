using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scantry.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAddedOnandScannedByfromtheProductentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseholdProducts_AspNetUsers_ScannedByUserId",
                table: "HouseholdProducts");

            migrationBuilder.DropIndex(
                name: "IX_HouseholdProducts_ScannedByUserId",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "ScannedByUserId",
                table: "HouseholdProducts");

            migrationBuilder.DropColumn(
                name: "ScannedOn",
                table: "HouseholdProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
