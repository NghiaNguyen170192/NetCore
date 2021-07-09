using Microsoft.Extensions.Configuration;
using NetCore.Infrastructure.Database;

namespace NetCore.Shared
{
    public static class ConfigurationHelper
    {
        public static DatabaseOptions GetDatabaseOptions(this IConfiguration configuration, string key)
        {
            var databaseOptions = new DatabaseOptions();
            configuration.GetSection(key).Bind(databaseOptions);
            return databaseOptions;
        }
    }
}
