using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrosia.Migrations
{
    /// <inheritdoc />
    public partial class AddTry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Adults",
                table: "booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Children",
                table: "booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "booking",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adults",
                table: "booking");

            migrationBuilder.DropColumn(
                name: "Children",
                table: "booking");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "booking");

            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "booking");
        }
    }
}
