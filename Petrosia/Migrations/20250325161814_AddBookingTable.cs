/*20250325161814_AddBookingTable.cs*/

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrosia.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "guest_id",
                table: "room",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)");

            migrationBuilder.AlterColumn<int>(
                name: "Room_Number",
                table: "room",
                type: "int",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    Booking_ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guest_ID = table.Column<int>(type: "int(11)", nullable: false),
                    Room_ID = table.Column<int>(type: "int(11)", nullable: false),
                    Check_In_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Check_Out_Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Booking_ID);
                    table.ForeignKey(
                        name: "fk_booking_guest",
                        column: x => x.Guest_ID,
                        principalTable: "guest",
                        principalColumn: "Guest_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_booking_room",
                        column: x => x.Room_ID,
                        principalTable: "room",
                        principalColumn: "Room_ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_booking_Guest_ID",
                table: "booking",
                column: "Guest_ID");

            migrationBuilder.CreateIndex(
                name: "IX_booking_Room_ID",
                table: "booking",
                column: "Room_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.AlterColumn<int>(
                name: "guest_id",
                table: "room",
                type: "int(11)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Room_Number",
                table: "room",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
