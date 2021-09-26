using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace NetCore.Infrastructure.Migrations
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> :
    IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly string _connectionStringName;
        private readonly string _migrationsAssemblyName;

        public DesignTimeDbContextFactoryBase(string connectionStringName, string migrationsAssemblyName)
        {
            _connectionStringName = connectionStringName;
            _migrationsAssemblyName = migrationsAssemblyName;
        }

        public TContext CreateDbContext(string[] args)
        {
            return Create(Directory.GetCurrentDirectory(), Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                _connectionStringName, _migrationsAssemblyName);
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        public TContext CreateWithConnectionStringName(string connectionStringName, string migrationsAssemblyName)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName, connectionStringName, migrationsAssemblyName);
        }

        private TContext Create(string basePath, string environmentName, string connectionStringName, string migrationsAssemblyName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connecionString = config.GetConnectionString(connectionStringName);

            if (String.IsNullOrWhiteSpace(connecionString) == true)
            {
                throw new InvalidOperationException("Could not find a connection string named 'default'.");
            }

            return CreateWithConnectionString(connecionString, migrationsAssemblyName);
        }

        private TContext CreateWithConnectionString(string connectionString, string migrationsAssemblyName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly(migrationsAssemblyName));
            DbContextOptions<TContext> options = optionsBuilder.Options;

            return CreateNewInstance(options);
        }
    }
}
