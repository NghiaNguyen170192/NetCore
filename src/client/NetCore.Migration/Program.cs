using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Migration;
using NetCore.Migration.Extensions;
using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.Extensions;
using Microsoft.Extensions.Configuration;
using NetCore.Infrastructure.Database.AppSettingConfigurations;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
var host = Host
	.CreateDefaultBuilder(args)
	.UseEnvironment(environment)
	.ConfigureAppConfiguration((context, builder)
		=> builder.AddAppSettings(environment, args))
	.ConfigureServices((context, services) =>
	{
		var databaseConfiguration = new DatabaseConfiguration();
		context.Configuration.GetSection("Database").Bind(databaseConfiguration);
		services.AddApplication();
		services.AddInfrastructure(databaseConfiguration);
		services.AddMigrationService();
	})
	.AddLogger("netcore-migration-logs")
	.Build();

using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<MigrationService>();
await service.RunAsync(args);