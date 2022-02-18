using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using System;
using System.Globalization;
using System.IO;

namespace NetCore.Shared.Extentions
{
    public static class LoggingConfiguration
    {
        private const string DEFAULT_FILE_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
        private static readonly MessageTemplateTextFormatter _formatter = new(DEFAULT_FILE_OUTPUT_TEMPLATE, CultureInfo.InvariantCulture);
      
        public static IHostBuilder AddLoggingConfiguration(this IHostBuilder hostBuilder, string applicationName)
        {
            return hostBuilder.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration
                   .Enrich.FromLogContext()
                   .WriteTo.Async(loggerSinkConfiguration =>
                   {
                       var logLevel = hostBuilderContext.Configuration.GetValue<LogEventLevel>("Logging:LogLevel:Default");
                       var logFileDirectory = hostBuilderContext.Configuration.GetValue<string>("Logging:Path");
                       var logFileName = $"{DateTime.Now:yyyy-MM-dd}.log";
                       var logFilePath = Path.Combine(logFileDirectory, applicationName,  logFileName);

                       loggerSinkConfiguration.File(formatter: _formatter,path: logFilePath,restrictedToMinimumLevel: logLevel, rollingInterval: RollingInterval.Day, shared: true);
                       loggerSinkConfiguration.Console(logLevel, DEFAULT_FILE_OUTPUT_TEMPLATE);
                   }));
        }
    }
}
