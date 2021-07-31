using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using NetCore.Infrastructure.Database.Contexts;

namespace NetCore.Infrastructure.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var sqlConnection = new SqlConnection();
            var options = Options.Create(new DatabaseOptions());
            return CreateDbContext(args, sqlConnection, options);
        }

        public DatabaseContext CreateDbContext(string[] args, SqlConnection sqlConnection, IOptions<DatabaseOptions> options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            var migrationsAssembly = options.Value.MigrationsAssembly ?? GetType().Assembly.FullName;
            optionsBuilder.UseSqlServer(sqlConnection, o => o.MigrationsAssembly(migrationsAssembly));

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
