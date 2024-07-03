using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TestSystem.Core;

public class TestDbMigrationContextFactory : IDesignTimeDbContextFactory<TestDbMigrationContext>
{
    // Holds migration infrastructure settings
    private const string AppSettingsFilePath = "appsettings.json";

    public TestDbMigrationContext CreateDbContext(string[] args)
    {
        Console.WriteLine("Created db context");
        return new TestDbMigrationContext(GetDbContextOptions());
    }

    public static DbContextOptions<TestDbMigrationContext> GetDbContextOptions()
    {
        Console.WriteLine("Starting migrations...");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSettingsFilePath)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
            .Build();

        var connectionString = configuration.GetConnectionString("TestManagementDbConnection");

        Console.WriteLine($"Attempting to run migrations with connection: '{connectionString}'");
        
        var dbContextBuilder =
            new DbContextOptionsBuilder<TestDbMigrationContext>().UseSqlServer(connectionString);

        Console.WriteLine("Created db context options");

        return dbContextBuilder.Options;
    }
}