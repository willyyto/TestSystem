
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class QuestionResultConfig : IEntityTypeConfiguration<QuestionResult>
{
    public void Configure(EntityTypeBuilder<QuestionResult> builder)
    {
        builder.ToTable(nameof(QuestionResult));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Answer).IsRequired().HasMaxLength(10000); // JSON for complex answers
        builder.Property(e => e.IsCorrect).IsRequired();
        builder.Property(e => e.PointsEarned).IsRequired().HasPrecision(8, 2);
        builder.Property(e => e.MaxPoints).IsRequired().HasPrecision(8, 2);
        builder.Property(e => e.IsSkipped).HasDefaultValue(false);
        builder.Property(e => e.RequiresManualGrading).HasDefaultValue(false);
        builder.Property(e => e.InstructorFeedback).HasMaxLength(2000);
        builder.Property(e => e.FileSubmissionPath).HasMaxLength(1000);

        builder.HasOne(e => e.Question)
            .WithMany(q => q.QuestionResults)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TestResult)
            .WithMany(tr => tr.QuestionResults)
            .HasForeignKey(e => e.TestResultId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.QuestionId);
        builder.HasIndex(e => e.TestResultId);
        builder.HasIndex(e => e.IsCorrect);
        builder.HasIndex(e => e.RequiresManualGrading);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}