using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiLanguageExamManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ExamReulst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "ExamResult",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "ExamResult",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "ExamResult");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "ExamResult");
        }
    }
}
