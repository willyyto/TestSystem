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

        builder.HasOne(e => e.Test)
            .WithMany(t => t.Questions)
            .HasForeignKey(e => e.TestId);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}