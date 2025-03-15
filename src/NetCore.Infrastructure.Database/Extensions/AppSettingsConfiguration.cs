using Microsoft.Extensions.Configuration;

namespace NetCore.Infrastructure.Database.Extensions;

public static class AppSettingsConfiguration
{
	public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configurationBuilder, string environment, string[] args)
	{
		configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
		configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
		configurationBuilder.AddEnvironmentVariables();
		configurationBuilder.AddCommandLine(args);

		return configurationBuilder;
	}
}