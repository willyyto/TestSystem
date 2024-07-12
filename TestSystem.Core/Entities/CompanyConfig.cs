using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class CompanyConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable(nameof(Company));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Name).IsRequired();

        builder.HasMany(e => e.Tests)
            .WithOne(t => t.Company)
            .HasForeignKey(t => t.CompanyId);
        
        builder.HasMany(e => e.Users)
            .WithOne(u => u.Company)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}