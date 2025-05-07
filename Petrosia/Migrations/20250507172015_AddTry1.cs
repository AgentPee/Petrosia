using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrosia.Migrations
{
    /// <inheritdoc />
    public partial class AddTry1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "booking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "booking",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
