using Microsoft.Extensions.Configuration;
using System.IO;

namespace NetCore.Infrastructure.Services
{
    public static class ConnectionstringService
    {
        public static string GetConnectionstring()
        {
            string connectionstring;
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            connectionstring = configuration.GetConnectionString("DefaultConnection");
            
            return connectionstring;
        }
    }
}
