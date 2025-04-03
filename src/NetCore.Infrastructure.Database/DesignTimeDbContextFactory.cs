using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using NetCore.Infrastructure.Database.AppSettingConfigurations;

namespace NetCore.Infrastructure.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDatabaseContext>
{
	public ApplicationDatabaseContext CreateDbContext(string[] args)
	{
		var sqlConnection = new SqlConnection();
		var databaseConfiguration = Options.Create(new DatabaseConfiguration());
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDatabaseContext>();
		var migrationsAssembly = databaseConfiguration.Value.MigrationsAssembly ?? GetType().Assembly.FullName;

		optionsBuilder.UseSqlServer(sqlConnection, o => o.MigrationsAssembly(migrationsAssembly));

		return new ApplicationDatabaseContext(optionsBuilder.Options);
	}
}