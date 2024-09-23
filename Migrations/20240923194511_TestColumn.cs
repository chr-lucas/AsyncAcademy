using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncAcademy.Migrations
{
    /// <inheritdoc />
    public partial class TestColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Test",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Users");
        }
    }
}
