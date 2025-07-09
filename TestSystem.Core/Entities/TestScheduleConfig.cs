using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class TestScheduleConfig : IEntityTypeConfiguration<TestSchedule>
{
    public void Configure(EntityTypeBuilder<TestSchedule> builder)
    {
        builder.ToTable(nameof(TestSchedule));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.StartDateTime).IsRequired();
        builder.Property(e => e.EndDateTime).IsRequired();
        builder.Property(e => e.TimeZone).HasMaxLength(100);
        builder.Property(e => e.IsRecurring).HasDefaultValue(false);
        builder.Property(e => e.RecurrencePattern).HasMaxLength(1000); // JSON
        builder.Property(e => e.MaxParticipants).HasDefaultValue(0);

        builder.HasOne(e => e.Test)
            .WithMany(t => t.Schedules)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.TestId);
        builder.HasIndex(e => e.StartDateTime);
        builder.HasIndex(e => e.EndDateTime);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}