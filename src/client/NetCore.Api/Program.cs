using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NetCore.Shared.Extensions;

namespace NetCore.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateWebHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) => 
                configurationBuilder.AddAppSettings(hostBuilderContext, args))
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .AddLoggingConfiguration("netcore-api");
    }
}