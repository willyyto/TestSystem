using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class CompanyConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable(nameof(Company));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.Website).HasMaxLength(500);
        builder.Property(e => e.LogoUrl).HasMaxLength(1000);
        builder.Property(e => e.Address).HasMaxLength(500);
        builder.Property(e => e.City).HasMaxLength(100);
        builder.Property(e => e.State).HasMaxLength(100);
        builder.Property(e => e.Country).HasMaxLength(100);
        builder.Property(e => e.PostalCode).HasMaxLength(20);
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.Property(e => e.Email).HasMaxLength(320);
        builder.Property(e => e.ContactPerson).HasMaxLength(200);
        
        // Subscription properties
        builder.Property(e => e.SubscriptionTier).HasMaxLength(50).HasDefaultValue("Free");
        builder.Property(e => e.MaxUsers).HasDefaultValue(10);
        builder.Property(e => e.MaxTests).HasDefaultValue(5);
        builder.Property(e => e.MaxQuestionsPerTest).HasDefaultValue(50);
        builder.Property(e => e.CustomBrandingEnabled).HasDefaultValue(false);
        builder.Property(e => e.AdvancedReportsEnabled).HasDefaultValue(false);
        builder.Property(e => e.ApiAccessEnabled).HasDefaultValue(false);
        builder.Property(e => e.StorageLimitMB).HasDefaultValue(100);
        builder.Property(e => e.StorageUsedMB).HasDefaultValue(0);
        
        // Settings
        builder.Property(e => e.CustomCss).HasMaxLength(10000);
        builder.Property(e => e.CustomDomain).HasMaxLength(255);
        builder.Property(e => e.SmtpSettings).HasMaxLength(2000); // JSON
        builder.Property(e => e.Settings).HasMaxLength(5000); // JSON

        builder.HasMany(e => e.Tests)
            .WithOne(t => t.Company)
            .HasForeignKey(t => t.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Users)
            .WithOne(u => u.Company)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Email);
        builder.HasIndex(e => e.SubscriptionTier);
        builder.HasIndex(e => e.CustomDomain).IsUnique();

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}