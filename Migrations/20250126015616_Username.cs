using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziekanatBackend.Migrations
{
    /// <inheritdoc />
    public partial class Username : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Lecturer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Lecturer");
        }
    }
}
