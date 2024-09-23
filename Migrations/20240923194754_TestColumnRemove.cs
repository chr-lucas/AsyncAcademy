using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncAcademy.Migrations
{
    /// <inheritdoc />
    public partial class TestColumnRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
