using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scantry.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImplementTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the Ticket entity.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Current status of the ticket; changed by admin."),
                    TicketType = table.Column<int>(type: "int", nullable: false, comment: "Ticket type; initially set by user."),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Problem description; provided by user."),
                    AdminResponse = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Admin response to ticket."),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Ticket create date."),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Ticket resolve date; added on status Rejected or Completed."),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true, comment: "Id of the User that created the ticket."),
                    AssignedToAdminId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true, comment: "Id of the Admin working on the ticket."),
                    ProductId = table.Column<int>(type: "int", nullable: true, comment: "Id of the reported Product, if applicable."),
                    HouseholdId = table.Column<int>(type: "int", nullable: true, comment: "Id of the reported Household, if applicable.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_AssignedToAdminId",
                        column: x => x.AssignedToAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tickets_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tickets_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToAdminId",
                table: "Tickets",
                column: "AssignedToAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedByUserId",
                table: "Tickets",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_HouseholdId",
                table: "Tickets",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProductId",
                table: "Tickets",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
