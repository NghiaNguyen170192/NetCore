using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace NetCore.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
        foreach (var type in typesFromAssemblies)
            services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
    }

    public static IServiceCollection RegisterClassesFromAssemblyInterface<T>(this IServiceCollection services)
    {
        services.Scan(selector => selector
             .FromAssemblyOf<T>()
             .AddClasses(classes => classes
                 .AssignableTo<T>())
                 .AsImplementedInterfaces()
                 .WithScopedLifetime());

        return services;
    }
}
