using Microsoft.EntityFrameworkCore;
using TestSystem;
using TestSystem.Core;
using TestSystem.Extensions; // Add this
using TestSystem.Filters; // Add this

// retrieve and inject the application configuration
var appConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
    .Build();

// create the application
var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

// Configure the host container (Autofac) within this method
startup.ConfigureHostContainer(builder.Host, appConfig);

// Configure the global Microsoft container services
startup.ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();

// Configure the app and web request pipeline
startup.Configure(app, builder.Environment);

// Add the API response middleware BEFORE other middleware
app.UseApiResponseMiddleware(); // Add this line

// only attempt to auto-run migrations outside of development environs to speed up build-times
if (!builder.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var migrationDbContext = scope.ServiceProvider.GetRequiredService<TestDbMigrationContext>();
    migrationDbContext.Database.Migrate();
}

app.Run();