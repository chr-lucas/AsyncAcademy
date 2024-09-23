using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncAcademy.Migrations
{
    /// <inheritdoc />
    public partial class updateToSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Sections");
        }
    }
}
