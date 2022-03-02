using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Infrastructure.Database;
using NetCore.Shared.Extentions;
using System;
using System.Threading.Tasks;

namespace NetCore.Tools.Migration
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            try
            {
                var application = scope.ServiceProvider.GetRequiredService<MigrationService>();
                await application.RunAsync(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .ConfigureAppConfiguration((HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder) => configurationBuilder.AddAppSettings(hostBuilderContext, args))
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    var databaseOptions = new DatabaseOptions();
                    hostBuilderContext.Configuration.GetSection("ConnectionStrings").Bind(databaseOptions);

                    services.AddDbContext<DatabaseContext>(builder =>
                    {
                        builder.UseSqlServer(databaseOptions.ApplicationConnectionString, o => o.MigrationsAssembly(databaseOptions.MigrationsAssembly));
                    });

                    services.AddSingleton<MigrationService>();
                })
                .AddLoggingConfiguration("netcore-migration-logs"); ;
        }
    }
}
