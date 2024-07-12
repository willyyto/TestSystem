using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities
{
    public class TestConfig : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable(nameof(Test));

            builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(1000); // Configuring description
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
            builder.Property(e => e.Duration).IsRequired();
            builder.Property(e => e.PassMark).IsRequired();
            builder.Property(e => e.IsTimed).IsRequired();
            builder.Property(e => e.ShuffleQuestions).IsRequired();
            builder.Property(e => e.MaximumAttempts).IsRequired();
            builder.Property(e => e.Visibility).IsRequired();
            builder.Property(e => e.TestType).IsRequired();
            builder.Property(e => e.Instructions).HasMaxLength(2000); // Configuring instructions
            builder.Property(e => e.Feedback).IsRequired();
            builder.Property(e => e.TestAccessControl).IsRequired();
            builder.Property(e => e.GradingScheme).IsRequired();

            builder.OwnsOne(e => e.RetakePolicy, rp =>
            {
                rp.Property(r => r.AllowRetakes).IsRequired().HasDefaultValue(false);
                rp.Property(r => r.MaxRetakes).IsRequired().HasDefaultValue(1);
                rp.Property(r => r.RetakeInterval).IsRequired().HasDefaultValue(TimeSpan.FromDays(1));
            });

            builder.HasOne(e => e.Company)
                .WithMany(c => c.Tests)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.TestResults)
                .WithOne(tr => tr.Test)
                .HasForeignKey(tr => tr.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
        }
    }
}
