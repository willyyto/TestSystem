using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class TestResultConfig : IEntityTypeConfiguration<TestResult>
{
    public void Configure(EntityTypeBuilder<TestResult> builder)
    {
        builder.ToTable(nameof(TestResult));

        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.CompletedDate).IsRequired();
        builder.Property(e => e.Score).IsRequired();

        builder.HasOne(e => e.Test)
            .WithMany(t => t.TestResults)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.QuestionResults)
            .WithOne(qr => qr.TestResult)
            .HasForeignKey(qr => qr.TestResultId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.TestId);

        // Additional configurations if needed
    }
}