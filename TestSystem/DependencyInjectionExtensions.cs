using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestSystem.Core;
using TestSystem.Core.Entities;
using TestSystem.Infra;
using TestSystem.Infra.DataServices;

namespace TestSystem;

public static class EfContextRegistrationExtensions
{
    /// <summary>
    ///     Add the application layer services
    /// </summary>
    /// <param name="containerBuilder"></param>
    /// <returns></returns>
    public static ContainerBuilder AddApplicationServices(this ContainerBuilder containerBuilder)
    {
        containerBuilder
            .RegisterType<CancellationTokenAccessor>()
            .As<ICancellationTokenAccessor>()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterAttributeTaggedServices<InstanceScopedServiceAttribute>();
        containerBuilder.RegisterAttributeTaggedServices<InstanceScopedBusinessServiceAttribute>();
        containerBuilder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>().SingleInstance();


        return containerBuilder;
    }

    /// <summary>
    ///     Register any services tagged with the instance registration attribute
    /// </summary>
    /// <param name="assembly">The assembly to search (passing the tag's assembly is an easy start)</param>
    /// <seealso cref="InstanceScopedServiceAttribute" />
    private static ContainerBuilder RegisterAttributeTaggedServices<T>(this ContainerBuilder containerBuilder)
        where T : Attribute
    {
        containerBuilder.RegisterAssemblyTypes(typeof(T).Assembly)
            .Where(type => type.GetCustomAttributes(typeof(T), false).Any())
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        return containerBuilder;
    }

    /// <summary>
    ///     Add the relevant EF Core db contexts
    /// </summary>
    public static ContainerBuilder AddEfCoreDbContexts(this ContainerBuilder builder)
    {
        return builder
            .AddTestSystemDbContext()
            .AddManagementMigrationsDbContext();
    }

    /// <summary>
    ///     Configure the ef core database (sets the db connection string)
    /// </summary>
    public static ContainerBuilder AddDatabaseSettings(this ContainerBuilder containerBuilder, IConfiguration config)
    {
        var databaseSettings = new DatabaseSettings(
            config.GetConnectionString("TestSystemDbConnection")
        );

        containerBuilder.RegisterInstance(databaseSettings).AsSelf().SingleInstance();

        return containerBuilder;
    }

    private static ContainerBuilder AddDbContextOptions<TContext>(this ContainerBuilder containerBuilder)
        where TContext : DbContext
    {
        containerBuilder.Register(sp =>
            {
                var loggerFactory = sp.Resolve<ILoggerFactory>();
                var dbSettings = sp.Resolve<DatabaseSettings>();
                return new DbContextOptionsBuilder<TContext>()
                    .UseLoggerFactory(loggerFactory)
                    .UseSqlServer(dbSettings.ConnectionString)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .Options; // make sure to return options here! Otherwise we'll register the builder
            })
            .AsSelf()
            .SingleInstance();

        return containerBuilder;
    }

    private static ContainerBuilder AddTestSystemDbContext(this ContainerBuilder builder)
    {
        builder
            .AddDbContextOptions<TestSystemDbContextAsync>()
            .RegisterType<TestSystemDbContextAsync>()
            .As<ITestSystemDbContextAsync>()
            .InstancePerLifetimeScope();

        return builder;
    }

    private static ContainerBuilder AddManagementMigrationsDbContext(this ContainerBuilder builder)
    {
        builder
            .RegisterType<TestDbMigrationContext>()
            .WithParameter("opts", TestDbMigrationContextFactory.GetDbContextOptions())
            .InstancePerLifetimeScope();

        return builder;
    }
}