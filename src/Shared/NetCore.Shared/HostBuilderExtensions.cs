using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace NetCore.Shared
{
    public static class HostBuilderExtensions
    {
        private const string ConfigureServicesMethodName = "ConfigureServices";

        /// <summary>
        /// Specify the startup type to be used by the host.
        /// </summary>
        /// <typeparam name="TStartup">The type containing an optional constructor with
        /// an <see cref="IConfiguration"/> parameter. The implementation should contain a public
        /// method named ConfigureServices with <see cref="IServiceCollection"/> parameter.</typeparam>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to initialize with TStartup.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder UseStartup<TStartup>(
            this IHostBuilder hostBuilder) where TStartup : class
        {
            // Invoke the ConfigureServices method on IHostBuilder...
            hostBuilder.ConfigureServices((ctx, serviceCollection) =>
            {
                // Find a method that has this signature: ConfigureServices(IServiceCollection)
                var configServicesMethod = typeof(TStartup).GetMethod(
                    ConfigureServicesMethodName, new Type[] { typeof(IServiceCollection) });

                var hasConfigConstructor = typeof(TStartup).GetConstructor(
                    new Type[] { typeof(IConfiguration) }) != null;

                // create a TStartup instance based on ctor
                var startUpObject = hasConfigConstructor ?
                    (TStartup)Activator.CreateInstance(typeof(TStartup), ctx.Configuration) :
                    (TStartup)Activator.CreateInstance(typeof(TStartup), null);

                // finally, call the ConfigureServices implemented by the TStartup object
                configServicesMethod?.Invoke(startUpObject, new object[] { serviceCollection });
            });

            // chain the response
            return hostBuilder;
        }
    }
}
