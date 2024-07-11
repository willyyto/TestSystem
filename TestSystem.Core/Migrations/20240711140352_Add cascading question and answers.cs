using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class Addcascadingquestionandanswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "TestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TestResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "TestResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "TestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "QuestionResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "QuestionResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "QuestionResult");
        }
    }
}
