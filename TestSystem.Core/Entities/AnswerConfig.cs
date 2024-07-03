using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class AnswerConfig : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable(nameof(Answer));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Text).IsRequired();
        builder.Property(e => e.IsCorrect).IsRequired();

        builder.HasOne(e => e.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(e => e.QuestionId);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}