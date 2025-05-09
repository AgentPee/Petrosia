using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrosia.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_room_RoomId",
                table: "bookings");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "bookings",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)");

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_room_RoomId",
                table: "bookings",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "Room_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_room_RoomId",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "bookings");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "bookings",
                type: "int(11)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_room_RoomId",
                table: "bookings",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "Room_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
