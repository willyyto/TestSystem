using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Core.Entities;

public class TestConfig : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.ToTable(nameof(Test));

        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.Instructions).HasMaxLength(5000);
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired();
        builder.Property(e => e.Duration).IsRequired();
        builder.Property(e => e.PassMark).IsRequired();
        builder.Property(e => e.IsTimed).IsRequired();
        builder.Property(e => e.ShuffleQuestions).IsRequired();
        builder.Property(e => e.MaximumAttempts).IsRequired();
        builder.Property(e => e.Visibility).IsRequired();
        builder.Property(e => e.TestType).IsRequired();
        builder.Property(e => e.Feedback).IsRequired();
        builder.Property(e => e.TestAccessControl).IsRequired();
        builder.Property(e => e.GradingScheme).IsRequired();
        
        // Enhanced properties
        builder.Property(e => e.ShowProgressBar).HasDefaultValue(true);
        builder.Property(e => e.AllowBackNavigation).HasDefaultValue(true);
        builder.Property(e => e.ShowQuestionNumbers).HasDefaultValue(true);
        builder.Property(e => e.AutoSubmit).HasDefaultValue(false);
        builder.Property(e => e.RequirePassword).HasDefaultValue(false);
        builder.Property(e => e.Password).HasMaxLength(100);
        builder.Property(e => e.ShowResultsImmediately).HasDefaultValue(true);
        builder.Property(e => e.ShowCorrectAnswers).HasDefaultValue(true);
        builder.Property(e => e.ShowScorePercentage).HasDefaultValue(true);
        builder.Property(e => e.EmailResults).HasDefaultValue(false);
        builder.Property(e => e.CustomCss).HasMaxLength(10000);
        builder.Property(e => e.WelcomeMessage).HasMaxLength(2000);
        builder.Property(e => e.CompletionMessage).HasMaxLength(2000);
        builder.Property(e => e.FailureMessage).HasMaxLength(2000);
        builder.Property(e => e.IsPublic).HasDefaultValue(false);
        builder.Property(e => e.InviteCode).HasMaxLength(50);
        builder.Property(e => e.RandomizeFromPool).HasDefaultValue(false);
        builder.Property(e => e.DisableCopyPaste).HasDefaultValue(false);
        builder.Property(e => e.FullScreenMode).HasDefaultValue(false);
        builder.Property(e => e.DisableRightClick).HasDefaultValue(false);
        builder.Property(e => e.TrackTabSwitches).HasDefaultValue(false);
        builder.Property(e => e.MaxTabSwitches).HasDefaultValue(3);
        builder.Property(e => e.RequireWebcam).HasDefaultValue(false);
        builder.Property(e => e.RequireMicrophone).HasDefaultValue(false);
        builder.Property(e => e.EnableScreenRecording).HasDefaultValue(false);
        builder.Property(e => e.IsScheduled).HasDefaultValue(false);

        builder.OwnsOne(e => e.RetakePolicy, rp =>
        {
            rp.Property(r => r.AllowRetakes).IsRequired().HasDefaultValue(false);
            rp.Property(r => r.MaxRetakes).IsRequired().HasDefaultValue(1);
            rp.Property(r => r.RetakeInterval).IsRequired().HasDefaultValue(TimeSpan.FromDays(1));
            rp.Property(r => r.RequirePasswordForRetake).HasDefaultValue(false);
            rp.Property(r => r.ResetProgressOnRetake).HasDefaultValue(true);
            rp.Property(r => r.ShowPreviousResults).HasDefaultValue(false);
            rp.Property(r => r.RetakePenalty).HasDefaultValue(0m).HasPrecision(5, 2);
        });

        builder.HasOne(e => e.Company)
            .WithMany(c => c.Tests)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.TestResults)
            .WithOne(tr => tr.Test)
            .HasForeignKey(tr => tr.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.TestAttempts)
            .WithOne(ta => ta.Test)
            .HasForeignKey(ta => ta.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CompanyId);
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);
        builder.HasIndex(e => e.TestType);
        builder.HasIndex(e => e.IsPublic);
        builder.HasIndex(e => e.InviteCode);

        builder.ConfigureMetaData().ConfigureArchivable().ConfigureActive();
    }
}