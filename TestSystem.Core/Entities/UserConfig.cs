using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities
{
    public class UserConfig : IEntityTypeConfiguration<User>
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
}