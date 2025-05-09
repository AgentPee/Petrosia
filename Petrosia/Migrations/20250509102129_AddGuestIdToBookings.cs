using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrosia.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestIdToBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_booking_guest",
                table: "booking");

            migrationBuilder.DropForeignKey(
                name: "fk_booking_room",
                table: "booking");

            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "booking");

            migrationBuilder.RenameTable(
                name: "booking",
                newName: "bookings");

            migrationBuilder.RenameColumn(
                name: "Room_ID",
                table: "bookings",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "Guest_ID",
                table: "bookings",
                newName: "GuestId");

            migrationBuilder.RenameColumn(
                name: "Check_Out_Date",
                table: "bookings",
                newName: "CheckOutDate");

            migrationBuilder.RenameColumn(
                name: "Check_In_Date",
                table: "bookings",
                newName: "CheckInDate");

            migrationBuilder.RenameColumn(
                name: "Booking_ID",
                table: "bookings",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_booking_Room_ID",
                table: "bookings",
                newName: "IX_bookings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_booking_Guest_ID",
                table: "bookings",
                newName: "IX_bookings_GuestId");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "bookings",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "bookings",
                type: "int(11)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(11)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOutDate",
                table: "bookings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInDate",
                table: "bookings",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int(11)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "bookings",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "bookings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BookingReference",
                table: "bookings",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "bookings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "bookings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "bookings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "bookings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "bookings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfAdults",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfNights",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "bookings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "bookings",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "bookings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequests",
                table: "bookings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "bookings",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Confirmed",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "bookings",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_guest_GuestId",
                table: "bookings",
                column: "GuestId",
                principalTable: "guest",
                principalColumn: "Guest_ID");

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
                name: "FK_bookings_guest_GuestId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_room_RoomId",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "BookingReference",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "City",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "NumberOfAdults",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "NumberOfNights",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "SpecialRequests",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "bookings");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "booking");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "booking",
                newName: "Room_ID");

            migrationBuilder.RenameColumn(
                name: "GuestId",
                table: "booking",
                newName: "Guest_ID");

            migrationBuilder.RenameColumn(
                name: "CheckOutDate",
                table: "booking",
                newName: "Check_Out_Date");

            migrationBuilder.RenameColumn(
                name: "CheckInDate",
                table: "booking",
                newName: "Check_In_Date");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "booking",
                newName: "Booking_ID");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_RoomId",
                table: "booking",
                newName: "IX_booking_Room_ID");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_GuestId",
                table: "booking",
                newName: "IX_booking_Guest_ID");

            migrationBuilder.AlterColumn<int>(
                name: "Room_ID",
                table: "booking",
                type: "int(11)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Guest_ID",
                table: "booking",
                type: "int(11)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int(11)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Check_Out_Date",
                table: "booking",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Check_In_Date",
                table: "booking",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "Booking_ID",
                table: "booking",
                type: "int(11)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "booking",
                column: "Booking_ID");

            migrationBuilder.AddForeignKey(
                name: "fk_booking_guest",
                table: "booking",
                column: "Guest_ID",
                principalTable: "guest",
                principalColumn: "Guest_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_booking_room",
                table: "booking",
                column: "Room_ID",
                principalTable: "room",
                principalColumn: "Room_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
