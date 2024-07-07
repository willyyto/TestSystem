using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class TestConfig : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.ToTable(nameof(Test));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Title).IsRequired();

        builder.HasOne(e => e.Company)
            .WithMany(c => c.Tests)
            .HasForeignKey(e => e.CompanyId);

        builder.HasMany(e => e.TestResults)
            .WithOne(tr => tr.Test)
            .HasForeignKey(tr => tr.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}