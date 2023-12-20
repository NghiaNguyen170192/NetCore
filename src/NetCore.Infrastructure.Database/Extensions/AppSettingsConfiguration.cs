using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NetCore.Infrastructure.Database.Extensions;

public static class AppSettingsConfiguration
{
	public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configurationBuilder, HostBuilderContext hostBuilderContext, string[] args)
	{
		configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
		configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		configurationBuilder.AddJsonFile($"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
		configurationBuilder.AddEnvironmentVariables();
		configurationBuilder.AddCommandLine(args);

		return configurationBuilder;
	}
}