using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.AuthenticationDatabase;
using System.Reflection;

namespace NetCore.Infrastructure.Migrations.PersistedGrantDb
{
    public class PersistedGrantContextDesignTimeFactory : DesignTimeDbContextFactoryBase<PersistedGrantDbContext>
    {
        public PersistedGrantContextDesignTimeFactory() : base("IdpConnectionString", typeof(AssemblyReference).GetTypeInfo().Assembly.GetName().Name)
        {
        }

        protected override PersistedGrantDbContext CreateNewInstance(DbContextOptions<PersistedGrantDbContext> options)
        {
            return new PersistedGrantDbContext(options, new OperationalStoreOptions());
        }
    }
}
