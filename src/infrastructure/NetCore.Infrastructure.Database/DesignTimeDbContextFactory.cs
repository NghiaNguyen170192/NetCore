using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace NetCore.Infrastructure.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var sqlConnection = new SqlConnection();
            var databaseConfigurations = Options.Create(new DatabaseConfigurations());
            return CreateDbContext(sqlConnection, databaseConfigurations);
        }

        public DatabaseContext CreateDbContext(SqlConnection sqlConnection, IOptions<DatabaseConfigurations> options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            var migrationsAssembly = options.Value.MigrationsAssembly ?? GetType().Assembly.FullName;
            optionsBuilder.UseSqlServer(sqlConnection, o => o.MigrationsAssembly(migrationsAssembly));

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
