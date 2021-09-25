using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace NetCore.Infrastructure.Migrations.ConfigurationDb
{
    public class ConfigurationContextDesignTimeFactory : DesignTimeDbContextFactoryBase<ConfigurationDbContext>
    {
        public ConfigurationContextDesignTimeFactory() : base("DefaultConnection", typeof(IStartup).GetTypeInfo().Assembly.GetName().Name)
        {

        }
        public ConfigurationContextDesignTimeFactory(string connectionStringName, string migrationsAssemblyName)
            : base(connectionStringName, migrationsAssemblyName)
        {
        }

        protected override ConfigurationDbContext CreateNewInstance(DbContextOptions<ConfigurationDbContext> options)
        {
            return new ConfigurationDbContext(options, new ConfigurationStoreOptions());
        }
    }
}
