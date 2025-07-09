using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class TestResultConfig : IEntityTypeConfiguration<TestResult>
{
    public void Configure(EntityTypeBuilder<TestResult> builder)
    {
        builder.ToTable(nameof(TestResult));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.CompletedDate).IsRequired();
        builder.Property(e => e.Score).IsRequired();
        builder.Property(e => e.RawScore).IsRequired().HasPrecision(10, 2);
        builder.Property(e => e.MaxPossibleScore).IsRequired().HasPrecision(10, 2);
        builder.Property(e => e.Grade).HasMaxLength(10).HasDefaultValue(string.Empty);
        builder.Property(e => e.Passed).IsRequired();
        builder.Property(e => e.TimeSpent).IsRequired();
        builder.Property(e => e.QuestionsAnswered).IsRequired();
        builder.Property(e => e.QuestionsCorrect).IsRequired();
        builder.Property(e => e.QuestionsSkipped).HasDefaultValue(0);
        builder.Property(e => e.Comments).HasMaxLength(2000);
        builder.Property(e => e.IsManuallyGraded).HasDefaultValue(false);
        builder.Property(e => e.CertificateUrl).HasMaxLength(1000);

        builder.HasOne(e => e.Test)
            .WithMany(t => t.TestResults)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.TestAttempt)
            .WithMany()
            .HasForeignKey(e => e.TestAttemptId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.QuestionResults)
            .WithOne(qr => qr.TestResult)
            .HasForeignKey(qr => qr.TestResultId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.TestId);
        builder.HasIndex(e => e.CompletedDate);
        builder.HasIndex(e => e.Score);
        builder.HasIndex(e => e.Passed);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}