using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class QuestionResultConfig : IEntityTypeConfiguration<QuestionResult>
{
    public void Configure(EntityTypeBuilder<QuestionResult> builder)
    {
        builder.ToTable(nameof(QuestionResult));

        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.IsCorrect).IsRequired();

        builder.HasOne(e => e.Question)
            .WithMany(q => q.QuestionResults)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.QuestionId);

        // Additional configurations if needed
    }
}