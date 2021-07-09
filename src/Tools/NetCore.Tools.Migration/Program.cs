using Microsoft.Extensions.Hosting;
using NetCore.Shared;

namespace NetCore.Tools.Migration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
			return Host.CreateDefaultBuilder(args).UseStartup<Startup>();
		}
    }
}
