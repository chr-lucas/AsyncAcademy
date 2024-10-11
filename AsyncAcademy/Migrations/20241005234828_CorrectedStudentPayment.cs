using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncAcademy.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedStudentPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentPayment",
                table: "StudentPayment");

            migrationBuilder.RenameTable(
                name: "StudentPayment",
                newName: "StudentPayment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentPayment",
                table: "StudentPayment",
                column: "StudentPaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentPayment",
                table: "StudentPayment");

            migrationBuilder.RenameTable(
                name: "StudentPayment",
                newName: "StudentPayment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentPayment",
                table: "StudentPayment",
                column: "StudentPaymentId");
        }
    }
}
