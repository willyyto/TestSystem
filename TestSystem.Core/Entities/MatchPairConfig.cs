using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities
{
    public class MatchPairConfig : IEntityTypeConfiguration<MatchPair>
    {
        public void Configure(EntityTypeBuilder<MatchPair> builder)
        {
            builder.ToTable(nameof(MatchPair));

            builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();

            builder.HasOne(e => e.Question)
                .WithMany(q => q.MatchPairs)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
        }
    }
}