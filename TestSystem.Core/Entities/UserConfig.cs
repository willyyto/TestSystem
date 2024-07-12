using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

// this implements the hierarchy as a TPH pattern, TPT is an alternative but often has worse performance (see here: https://learn.microsoft.com/en-us/ef/core/performance/modeling-for-performance#inheritance-mapping)
// see: https://learn.microsoft.com/en-us/ef/core/modeling/inheritance for more on the pattern and EF Core's implementation
public class UserConfig
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();

        builder.Property(e => e.Username).IsRequired();
        builder.Property(e => e.Password).IsRequired();
        
        builder.HasOne(e => e.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive().ConfigureLockable();
    }
}