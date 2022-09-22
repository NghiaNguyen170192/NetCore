using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using NetCore.Shared.Configurations;

namespace NetCore.Infrastructure.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var sqlConnection = new SqlConnection();
        var databaseConfiguration = Options.Create(new DatabaseConfiguration());
        return CreateDbContext(sqlConnection, databaseConfiguration);
    }

    public DatabaseContext CreateDbContext(SqlConnection sqlConnection, IOptions<DatabaseConfiguration> options)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

        var migrationsAssembly = options.Value.MigrationsAssembly ?? GetType().Assembly.FullName;
        optionsBuilder.UseSqlServer(sqlConnection, o => o.MigrationsAssembly(migrationsAssembly));

        return new DatabaseContext(optionsBuilder.Options);
    }
}
