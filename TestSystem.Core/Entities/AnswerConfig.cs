using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class AnswerConfig : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable(nameof(Answer));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Text).IsRequired().HasMaxLength(2000);
        builder.Property(e => e.IsCorrect).IsRequired();
        builder.Property(e => e.IsFillInTheBlank).IsRequired();
        builder.Property(e => e.ImageUrl).HasMaxLength(1000);
        builder.Property(e => e.Explanation).HasMaxLength(1000);
        builder.Property(e => e.Points).HasDefaultValue(1.0).HasPrecision(5, 2);
        builder.Property(e => e.IsCaseSensitive).HasDefaultValue(false);
        builder.Property(e => e.AcceptableAnswers).HasMaxLength(4000); // JSON array

        builder.HasOne(e => e.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.QuestionId);
        builder.HasIndex(e => e.IsCorrect);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}