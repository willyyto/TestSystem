using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class QuestionConfig : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable(nameof(Question));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Text).IsRequired().HasMaxLength(5000);
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.Weight).IsRequired().HasDefaultValue(1.0);
        builder.Property(e => e.TimeLimit).HasDefaultValue(0);
        builder.Property(e => e.IsRequired).HasDefaultValue(true);
        builder.Property(e => e.ImageUrl).HasMaxLength(1000);
        builder.Property(e => e.VideoUrl).HasMaxLength(1000);
        builder.Property(e => e.AudioUrl).HasMaxLength(1000);
        builder.Property(e => e.Explanation).HasMaxLength(2000);
        builder.Property(e => e.Hint).HasMaxLength(1000);
        builder.Property(e => e.DisplayOrder).HasDefaultValue(0);
        builder.Property(e => e.AllowMultipleAnswers).HasDefaultValue(false);
        builder.Property(e => e.ShuffleAnswers).HasDefaultValue(false);
        builder.Property(e => e.CorrectNumericalAnswer).HasPrecision(18, 6);
        builder.Property(e => e.NumericalTolerance).HasPrecision(18, 6);
        builder.Property(e => e.NumericalUnit).HasMaxLength(50);
        builder.Property(e => e.ScaleMinLabel).HasMaxLength(100);
        builder.Property(e => e.ScaleMaxLabel).HasMaxLength(100);
        builder.Property(e => e.AllowedFileTypes).HasMaxLength(500);
        builder.Property(e => e.OrderingInstructions).HasMaxLength(1000);

        builder.HasOne(e => e.Test)
            .WithMany(t => t.Questions)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.QuestionResults)
            .WithOne(qr => qr.Question)
            .HasForeignKey(qr => qr.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.MatchPairs)
            .WithOne(mp => mp.Question)
            .HasForeignKey(mp => mp.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.OrderingItems)
            .WithOne(oi => oi.Question)
            .HasForeignKey(oi => oi.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.TestId);
        builder.HasIndex(e => e.Type);
        builder.HasIndex(e => e.DisplayOrder);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}