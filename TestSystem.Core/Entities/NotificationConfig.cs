using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Type).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Message).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.IsRead).HasDefaultValue(false);
        builder.Property(e => e.ActionUrl).HasMaxLength(500);

        // Indexes for performance
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.IsRead);
        builder.HasIndex(e => e.CreatedOn);
        builder.HasIndex(e => new { e.UserId, e.IsRead });

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}