using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class Add_Quiz_Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationExpires",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailVerified",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "User",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                defaultValue: "en");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIp",
                table: "User",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationEmailEnabled",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationSmsEnabled",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetExpires",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "User",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "User",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "User",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "UTC");

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsArchived",
                table: "TestResult",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TestResult",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TestResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<string>(
                name: "CertificateUrl",
                table: "TestResult",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "TestResult",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "TestResult",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "GradedAt",
                table: "TestResult",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GradedBy",
                table: "TestResult",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManuallyGraded",
                table: "TestResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MaxPossibleScore",
                table: "TestResult",
                type: "float(10)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Passed",
                table: "TestResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsAnswered",
                table: "TestResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsCorrect",
                table: "TestResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsSkipped",
                table: "TestResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RawScore",
                table: "TestResult",
                type: "float(10)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "TestAttemptId",
                table: "TestResult",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpent",
                table: "TestResult",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<string>(
                name: "Visibility",
                table: "Test",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "Test",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "TestType",
                table: "Test",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TestAccessControl",
                table: "Test",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Test",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "Test",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "GradingScheme",
                table: "Test",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "Test",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Test",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Test",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "AllowBackNavigation",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AutoSubmit",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "Test",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableUntil",
                table: "Test",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletionMessage",
                table: "Test",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomCss",
                table: "Test",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisableCopyPaste",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableRightClick",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailResults",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableScreenRecording",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FailureMessage",
                table: "Test",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FullScreenMode",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InviteCode",
                table: "Test",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScheduled",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxTabSwitches",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Test",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RandomQuestionCount",
                table: "Test",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RandomizeFromPool",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireMicrophone",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePassword",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireWebcam",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RetakePolicy_RequirePasswordForRetake",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RetakePolicy_ResetProgressOnRetake",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RetakePolicy_RetakePenalty",
                table: "Test",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "RetakePolicy_ShowPreviousResults",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCorrectAnswers",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowProgressBar",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowQuestionNumbers",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowResultsImmediately",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowScorePercentage",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "TrackTabSwitches",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WelcomeMessage",
                table: "Test",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsArchived",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "QuestionResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<string>(
                name: "FileSubmissionPath",
                table: "QuestionResult",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorFeedback",
                table: "QuestionResult",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSkipped",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MaxPoints",
                table: "QuestionResult",
                type: "float(8)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PointsEarned",
                table: "QuestionResult",
                type: "float(8)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresManualGrading",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpent",
                table: "QuestionResult",
                type: "time",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "Question",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Question",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Question",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "AllowMultipleAnswers",
                table: "Question",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AllowedFileTypes",
                table: "Question",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "Question",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CorrectNumericalAnswer",
                table: "Question",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "Question",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hint",
                table: "Question",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Question",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "Question",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxFileSizeKB",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NumericalTolerance",
                table: "Question",
                type: "float(18)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumericalUnit",
                table: "Question",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderingInstructions",
                table: "Question",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScaleMax",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScaleMaxLabel",
                table: "Question",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScaleMin",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScaleMinLabel",
                table: "Question",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShuffleAnswers",
                table: "Question",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Question",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "Company",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Company",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Company",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Company",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdvancedReportsEnabled",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApiAccessEnabled",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Company",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Company",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Company",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBrandingEnabled",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomCss",
                table: "Company",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDomain",
                table: "Company",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Company",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Company",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Company",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxQuestionsPerTest",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 50);

            migrationBuilder.AddColumn<int>(
                name: "MaxTests",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<int>(
                name: "MaxUsers",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Company",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Company",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "Company",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpSettings",
                table: "Company",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Company",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StorageLimitMB",
                table: "Company",
                type: "bigint",
                nullable: false,
                defaultValue: 100L);

            migrationBuilder.AddColumn<long>(
                name: "StorageUsedMB",
                table: "Company",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionEnd",
                table: "Company",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionStart",
                table: "Company",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionTier",
                table: "Company",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Free");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Company",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answer",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AcceptableAnswers",
                table: "Answer",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "Answer",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Answer",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCaseSensitive",
                table: "Answer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Points",
                table: "Answer",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.CreateTable(
                name: "OrderingItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CorrectOrder = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderingItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderingItem_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestAttempt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeSpent = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsAbandoned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    TabSwitchCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ProctorData = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAttempt_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSchedule_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_EmailVerified",
                table: "User",
                column: "EmailVerified");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastLoginAt",
                table: "User",
                column: "LastLoginAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role",
                table: "User",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_CompletedDate",
                table: "TestResult",
                column: "CompletedDate");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_Passed",
                table: "TestResult",
                column: "Passed");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_Score",
                table: "TestResult",
                column: "Score");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_TestAttemptId",
                table: "TestResult",
                column: "TestAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_EndDate",
                table: "Test",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Test_InviteCode",
                table: "Test",
                column: "InviteCode");

            migrationBuilder.CreateIndex(
                name: "IX_Test_IsPublic",
                table: "Test",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_Test_StartDate",
                table: "Test",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Test_TestType",
                table: "Test",
                column: "TestType");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResult_IsCorrect",
                table: "QuestionResult",
                column: "IsCorrect");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResult_RequiresManualGrading",
                table: "QuestionResult",
                column: "RequiresManualGrading");

            migrationBuilder.CreateIndex(
                name: "IX_Question_DisplayOrder",
                table: "Question",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Type",
                table: "Question",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CustomDomain",
                table: "Company",
                column: "CustomDomain",
                unique: true,
                filter: "[CustomDomain] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Email",
                table: "Company",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                table: "Company",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Company_SubscriptionTier",
                table: "Company",
                column: "SubscriptionTier");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_IsCorrect",
                table: "Answer",
                column: "IsCorrect");

            migrationBuilder.CreateIndex(
                name: "IX_OrderingItem_CorrectOrder",
                table: "OrderingItem",
                column: "CorrectOrder");

            migrationBuilder.CreateIndex(
                name: "IX_OrderingItem_QuestionId",
                table: "OrderingItem",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_IsCompleted",
                table: "TestAttempt",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_StartedAt",
                table: "TestAttempt",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_TestId",
                table: "TestAttempt",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_TestId_UserId_AttemptNumber",
                table: "TestAttempt",
                columns: new[] { "TestId", "UserId", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_UserId",
                table: "TestAttempt",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSchedule_EndDateTime",
                table: "TestSchedule",
                column: "EndDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_TestSchedule_StartDateTime",
                table: "TestSchedule",
                column: "StartDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_TestSchedule_TestId",
                table: "TestSchedule",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResult_TestAttempt_TestAttemptId",
                table: "TestResult",
                column: "TestAttemptId",
                principalTable: "TestAttempt",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResult_TestAttempt_TestAttemptId",
                table: "TestResult");

            migrationBuilder.DropTable(
                name: "OrderingItem");

            migrationBuilder.DropTable(
                name: "TestAttempt");

            migrationBuilder.DropTable(
                name: "TestSchedule");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_EmailVerified",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_LastLoginAt",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Role",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Username",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_TestResult_CompletedDate",
                table: "TestResult");

            migrationBuilder.DropIndex(
                name: "IX_TestResult_Passed",
                table: "TestResult");

            migrationBuilder.DropIndex(
                name: "IX_TestResult_Score",
                table: "TestResult");

            migrationBuilder.DropIndex(
                name: "IX_TestResult_TestAttemptId",
                table: "TestResult");

            migrationBuilder.DropIndex(
                name: "IX_Test_EndDate",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_InviteCode",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_IsPublic",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_StartDate",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_TestType",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_QuestionResult_IsCorrect",
                table: "QuestionResult");

            migrationBuilder.DropIndex(
                name: "IX_QuestionResult_RequiresManualGrading",
                table: "QuestionResult");

            migrationBuilder.DropIndex(
                name: "IX_Question_DisplayOrder",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_Type",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Company_CustomDomain",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_Email",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_Name",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_SubscriptionTier",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Answer_IsCorrect",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailVerificationExpires",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailVerified",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastLoginIp",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NotificationEmailEnabled",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NotificationSmsEnabled",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordResetExpires",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CertificateUrl",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "GradedAt",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "GradedBy",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "IsManuallyGraded",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "MaxPossibleScore",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "Passed",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "QuestionsAnswered",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "QuestionsCorrect",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "QuestionsSkipped",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "RawScore",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "TestAttemptId",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "TestResult");

            migrationBuilder.DropColumn(
                name: "AllowBackNavigation",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "AutoSubmit",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "AvailableUntil",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "CompletionMessage",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "CustomCss",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "DisableCopyPaste",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "DisableRightClick",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "EmailResults",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "EnableScreenRecording",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "FailureMessage",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "FullScreenMode",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "IsScheduled",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "MaxTabSwitches",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RandomQuestionCount",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RandomizeFromPool",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RequireMicrophone",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RequirePassword",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RequireWebcam",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RetakePolicy_RequirePasswordForRetake",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RetakePolicy_ResetProgressOnRetake",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RetakePolicy_RetakePenalty",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "RetakePolicy_ShowPreviousResults",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ShowCorrectAnswers",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ShowProgressBar",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ShowQuestionNumbers",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ShowResultsImmediately",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ShowScorePercentage",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "TrackTabSwitches",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "WelcomeMessage",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "FileSubmissionPath",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "InstructorFeedback",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "IsSkipped",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "PointsEarned",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "RequiresManualGrading",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "QuestionResult");

            migrationBuilder.DropColumn(
                name: "AllowMultipleAnswers",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "AllowedFileTypes",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "CorrectNumericalAnswer",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Hint",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "MaxFileSizeKB",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "NumericalTolerance",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "NumericalUnit",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "OrderingInstructions",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ScaleMax",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ScaleMaxLabel",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ScaleMin",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ScaleMinLabel",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ShuffleAnswers",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AdvancedReportsEnabled",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "ApiAccessEnabled",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CustomBrandingEnabled",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CustomCss",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CustomDomain",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "MaxQuestionsPerTest",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "MaxTests",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "MaxUsers",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Settings",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SmtpSettings",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "StorageLimitMB",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "StorageUsedMB",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SubscriptionEnd",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SubscriptionStart",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SubscriptionTier",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AcceptableAnswers",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "IsCaseSensitive",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Answer");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "User",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(320)",
                oldMaxLength: 320);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "User",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsArchived",
                table: "TestResult",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TestResult",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TestResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<int>(
                name: "Visibility",
                table: "Test",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "Test",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "TestType",
                table: "Test",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TestAccessControl",
                table: "Test",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Test",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "Test",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradingScheme",
                table: "Test",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Feedback",
                table: "Test",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Test",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Test",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsArchived",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "QuestionResult",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "QuestionResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "Question",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Question",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Question",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                table: "Company",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Company",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }
    }
}
