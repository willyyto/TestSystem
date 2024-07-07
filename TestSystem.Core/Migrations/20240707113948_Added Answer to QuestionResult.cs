using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnswertoQuestionResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "QuestionResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "QuestionResult");
        }
    }
}
