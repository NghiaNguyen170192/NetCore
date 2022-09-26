using Microsoft.Extensions.DependencyInjection;
using NetCore.Shared.Extensions;
using NetCore.Migration.Common;
using NetCore.Migration.Common.Interface;

namespace NetCore.Migration.Extensions;

public static class MigrationDependencyInjection
{
    public static IServiceCollection RegisterSeeds(this IServiceCollection services)
    {
        services.AddSeedRunner<IBaseDataSeed>();
        services.AddSeedRunner<ITestDataSeed>();

        return services;
    }

    private static IServiceCollection AddSeedRunner<T>(this IServiceCollection services)
        where T : IDataSeed
    {
        services.AddSeedsType<T>();
        services.AddScoped<IDataSeedRunner<T>, DataSeedRunner<T>>();

        return services;
    }

    private static IServiceCollection AddSeedsType<T>(this IServiceCollection services)
        where T : IDataSeed
    {
        services.Scan(selector => selector
            .FromCallingAssembly()
            .AddClasses(classes => classes
                .AssignableTo<T>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }

    public static void RegisterMigrationTasks(this IServiceCollection services)
    {
        services.RegisterClassesFromAssemblyInterface<IMigrationTask>();
    }
}
