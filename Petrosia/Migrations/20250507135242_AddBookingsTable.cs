using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Petrosia.Migrations
{
    public partial class AddBookingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    ZipCode = table.Column<string>(maxLength: 20, nullable: false),
                    Country = table.Column<string>(maxLength: 50, nullable: false),
                    RoomType = table.Column<string>(maxLength: 50, nullable: false),
                    CheckInDate = table.Column<DateTime>(nullable: false),
                    CheckOutDate = table.Column<DateTime>(nullable: false),
                    NumberOfAdults = table.Column<int>(nullable: false),
                    NumberOfChildren = table.Column<int>(nullable: false),
                    NumberOfNights = table.Column<int>(nullable: false),
                    SpecialRequests = table.Column<string>(maxLength: 500, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PaymentMethod = table.Column<string>(maxLength: 50, nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    BookingReference = table.Column<string>(maxLength: 20, nullable: true),
                    Status = table.Column<string>(maxLength: 20, nullable: true, defaultValue: "Confirmed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.BookingId);
                });

            // Add index on Email for faster searches
            migrationBuilder.CreateIndex(
                name: "IX_bookings_Email",
                table: "bookings",
                column: "Email");

            // Add index on BookingReference for faster searches
            migrationBuilder.CreateIndex(
                name: "IX_bookings_BookingReference",
                table: "bookings",
                column: "BookingReference",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");
        }
    }
}