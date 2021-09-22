namespace NetCore.Infrastructure.Database
{
    public class DatabaseOptions
    {
        public string DatabaseConnection { get; set; }

        public string Provider { get; set; }

        public string MigrationsAssembly { get; set; }
    }
}