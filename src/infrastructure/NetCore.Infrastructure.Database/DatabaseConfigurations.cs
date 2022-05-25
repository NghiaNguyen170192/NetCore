namespace NetCore.Infrastructure.Database
{
    public class DatabaseConfigurations
    {
        public string ApplicationConnectionString { get; set; }

        public string IdpConnectionString { get; set; }

        public string Provider { get; set; }

        public string MigrationsAssembly { get; set; }

        public string RedisConnectionString { get; set; }
    }
}