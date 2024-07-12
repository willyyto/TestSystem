using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class QuestionConfig : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable(nameof(Question));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Text).IsRequired();
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.Weight).IsRequired().HasDefaultValue(1.0);

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

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}