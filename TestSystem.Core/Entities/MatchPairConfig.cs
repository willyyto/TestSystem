using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class MatchPairConfig : IEntityTypeConfiguration<MatchPair>
{
    public void Configure(EntityTypeBuilder<MatchPair> builder)
    {
        builder.ToTable(nameof(MatchPair));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.LeftItem).IsRequired();
        builder.Property(e => e.RightItem).IsRequired();
        builder.Property(e => e.LeftItemId).IsRequired().HasValueGenerator<IdGenerator>();
        builder.Property(e => e.RightItemId).IsRequired().HasValueGenerator<IdGenerator>();

        builder.HasOne(e => e.Question)
            .WithMany(q => q.MatchPairs)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}