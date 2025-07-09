using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class OrderingItemConfig : IEntityTypeConfiguration<OrderingItem>
{
    public void Configure(EntityTypeBuilder<OrderingItem> builder)
    {
        builder.ToTable(nameof(OrderingItem));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Text).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.CorrectOrder).IsRequired();

        builder.HasOne(e => e.Question)
            .WithMany(q => q.OrderingItems)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.QuestionId);
        builder.HasIndex(e => e.CorrectOrder);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}