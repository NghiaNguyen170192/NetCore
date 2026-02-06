using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Application.Messaging;
using NetCore.Domain.Messaging;
using NetCore.Infrastructure.Database;

namespace NetCore.Application.Tests;

public class BaseTest
{
    protected static async Task<ApplicationDatabaseContext> GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var services = new ServiceCollection();
        services.AddScoped<IDispatcher, Dispatcher>();
        var serviceProvider = services.BuildServiceProvider();

        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        var databaseContext = new ApplicationDatabaseContext(options, dispatcher);
        await databaseContext.Database.EnsureCreatedAsync();

        return databaseContext;
    }

    protected static IDispatcher GetDispatcher(IServiceProvider? serviceProvider = null)
    {
        if (serviceProvider != null)
        {
            return serviceProvider.GetRequiredService<IDispatcher>();
        }

        var services = new ServiceCollection();
        services.AddScoped<IDispatcher, Dispatcher>();
        return services.BuildServiceProvider().GetRequiredService<IDispatcher>();
    }
}