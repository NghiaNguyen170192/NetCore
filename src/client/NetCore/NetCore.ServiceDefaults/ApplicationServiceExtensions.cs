using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.AppSettingConfigurations;
using NetCore.Infrastructure.Database.Extensions;

namespace NetCore.ServiceDefaults;

public partial class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var databaseConfiguration = new DatabaseConfiguration();
        builder.Configuration.GetSection("Database").Bind(databaseConfiguration);
        
        //Dependency Injections
        builder.Services
            .AddApplication()
            .AddInfrastructure(databaseConfiguration);
    }
}
