using Microsoft.Extensions.Configuration;

namespace NetCore.Infrastructure.Database.Extensions
{
    public static class DatabaseExtensions
    {
        public static DatabaseOptions GetDatabaseOptions(this IConfiguration configuration, string key)
        {
            var databaseOptions = new DatabaseOptions();
            configuration.GetSection(key).Bind(databaseOptions);
            return databaseOptions;
        }
    }
}
