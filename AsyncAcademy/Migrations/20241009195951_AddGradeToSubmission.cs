using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncAcademy.Migrations
{
    /// <inheritdoc />
    public partial class AddGradeToSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsGraded",
                table: "Submissions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsGraded",
                table: "Submissions");
        }
    }
}
