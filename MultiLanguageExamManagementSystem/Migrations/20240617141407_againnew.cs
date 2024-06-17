using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiLanguageExamManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class againnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "possibleAnswers",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "possibleAnswers",
                table: "Questions");
        }
    }
}
