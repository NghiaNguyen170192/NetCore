namespace NetCore.Infrastructure.Database.Tests;

public class BaseTest
{
	protected static async Task<ApplicationDatabaseContext> GetContext()
	{
		var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
		.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
		.Options;

		var databaseContext = new ApplicationDatabaseContext(options, new NoMediator());
		await databaseContext.Database.EnsureCreatedAsync();

		return databaseContext;
	}
}
