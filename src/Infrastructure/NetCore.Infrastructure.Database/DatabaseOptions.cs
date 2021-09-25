namespace NetCore.Infrastructure.Database
{
    public class DatabaseOptions
    {
        public string ApplicationConnectionString { get; set; }

        public string IdpConnectionString { get; set; }

        public string Provider { get; set; }

        public string MigrationsAssembly { get; set; }
    }
}