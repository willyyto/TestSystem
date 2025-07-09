using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class TestAttemptConfig : IEntityTypeConfiguration<TestAttempt>
{
    public void Configure(EntityTypeBuilder<TestAttempt> builder)
    {
        builder.ToTable(nameof(TestAttempt));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.StartedAt).IsRequired();
        builder.Property(e => e.IsCompleted).HasDefaultValue(false);
        builder.Property(e => e.IsAbandoned).HasDefaultValue(false);
        builder.Property(e => e.AttemptNumber).IsRequired();
        builder.Property(e => e.UserAgent).HasMaxLength(500);
        builder.Property(e => e.IpAddress).HasMaxLength(45); // IPv6 support
        builder.Property(e => e.TabSwitchCount).HasDefaultValue(0);
        builder.Property(e => e.ProctorData).HasMaxLength(10000); // JSON

        builder.HasOne(e => e.Test)
            .WithMany(t => t.TestAttempts)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.TestId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.StartedAt);
        builder.HasIndex(e => e.IsCompleted);
        builder.HasIndex(e => new { e.TestId, e.UserId, e.AttemptNumber }).IsUnique();

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}