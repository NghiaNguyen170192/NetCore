using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database;

namespace NetCore.Application.Tests;

public class BaseTest
{
	protected static async Task<ApplicationDatabaseContext> GetContext()
	{
		var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
		.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
		.Options;

		var databaseContext = new ApplicationDatabaseContext(options);
		await databaseContext.Database.EnsureCreatedAsync();

		return databaseContext;
	}
}