using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Application.Behaviors;
using NetCore.Application.Messaging;
using NetCore.Domain.Messaging;

namespace NetCore.Application.Extensions;

/// <summary>
/// Extension methods for configuring CQRS dispatcher and handlers.
/// </summary>
public static class DispatcherServiceCollectionExtensions
{
    /// <summary>
    /// Adds the CQRS dispatcher and scans for handlers in the specified assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies to scan for handlers. If none provided, scans the calling assembly.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDispatcher(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            assemblies = new[] { Assembly.GetCallingAssembly() };
        }

        // Register the dispatcher
        services.AddScoped<IDispatcher, Dispatcher>();

        // Register all request handlers
        RegisterHandlers(services, assemblies);

        // Register pipeline behaviors
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        // Find all types that implement IRequestHandler<,>
        var handlerTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Select(t => new
            {
                Type = t,
                Interfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .ToList()
            })
            .Where(x => x.Interfaces.Any())
            .ToList();

        foreach (var handlerInfo in handlerTypes)
        {
            foreach (var handlerInterface in handlerInfo.Interfaces)
            {
                services.AddScoped(handlerInterface, handlerInfo.Type);
            }
        }
    }

    /// <summary>
    /// Adds a custom pipeline behavior.
    /// </summary>
    /// <typeparam name="TBehavior">The behavior type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddPipelineBehavior<TBehavior>(this IServiceCollection services)
        where TBehavior : class
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TBehavior));
        return services;
    }
}
