using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Migration;
using NetCore.Migration.Extensions;
using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Database.AppSettingConfigurations;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
var host = Host
	.CreateDefaultBuilder(args)
	.UseEnvironment(environment)
	.AddSharedConfiguration()
	.ConfigureServices((context, services) =>
	{
		var databaseConfiguration = context.Configuration.GetSection("Database").Get<DatabaseConfiguration>() ?? new();
		services.AddApplication();
		services.AddInfrastructure(context.Configuration);
		services.AddMigrationService();
	})
	.AddLogger("netcore-migration-logs")
	.Build();

using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<MigrationService>();
await service.RunAsync(args);