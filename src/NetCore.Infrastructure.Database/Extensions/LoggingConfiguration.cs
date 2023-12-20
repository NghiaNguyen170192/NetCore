using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace NetCore.Infrastructure.Database.Extensions;

public static class LoggingConfiguration
{
	private const string DefaultFileOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
	private static readonly MessageTemplateTextFormatter Formatter = new(DefaultFileOutputTemplate, CultureInfo.InvariantCulture);

	public static IHostBuilder AddLogger(this IHostBuilder hostBuilder, string applicationName)
	{
		return hostBuilder.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration
			   .Enrich.FromLogContext()
			   .WriteTo.Async(loggerSinkConfiguration =>
			   {
				   var logLevel = hostBuilderContext.Configuration.GetValue<LogEventLevel>("Logging:LogLevel:Default");
				   var logFileDirectory = hostBuilderContext.Configuration.GetValue<string>("Logging:Path");
				   var logFileName = $"{DateTime.Now:yyyy-MM-dd}.log";
				   var logFilePath = Path.Combine(logFileDirectory, applicationName, logFileName);

				   loggerSinkConfiguration.File(
					   formatter: Formatter,
					   path: logFilePath,
					   restrictedToMinimumLevel: logLevel,
					   rollingInterval: RollingInterval.Day,
					   shared: true);

				   loggerSinkConfiguration.Console(logLevel, DefaultFileOutputTemplate);
			   }));
	}
}