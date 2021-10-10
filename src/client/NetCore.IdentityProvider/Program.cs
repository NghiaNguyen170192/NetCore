using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NetCore.Shared.Extentions;

namespace NetCore.IdentityProvider
{
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
              .ConfigureAppConfiguration((HostBuilderContext hostbuilderContext, IConfigurationBuilder configurationBuilder) =>
                  configurationBuilder.AddAppSettings(hostbuilderContext, args))
              .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
              .AddLoggingConfiguration("netcore-idp");
        }
    }
}
