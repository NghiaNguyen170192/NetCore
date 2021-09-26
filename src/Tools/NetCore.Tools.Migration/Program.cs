using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Infrastructure.AuthenticationDatabase;
using NetCore.Infrastructure.Database;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Models.Identity;
using System;
using System.IO;

namespace NetCore.Tools.Migration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var application = scope.ServiceProvider.GetRequiredService<MigrationService>();
                    application.Run(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .ConfigureAppConfiguration((HostBuilderContext context, IConfigurationBuilder builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    builder.AddEnvironmentVariables();
                    builder.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var databaseOptions = hostContext.Configuration.GetDatabaseOptions("ConnectionStrings");
                    services.AddDbContext<DatabaseContext>(builder =>
                    {
                        builder.UseSqlServer(databaseOptions.ApplicationConnectionString, o => o.MigrationsAssembly(databaseOptions.MigrationsAssembly));
                    });

                    services.AddDbContext<ApplicationDbContext>(builder =>
                    {
                        builder.UseSqlServer(databaseOptions.IdpConnectionString, o => o.MigrationsAssembly(databaseOptions.MigrationsAssembly));
                    });

                    services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

                    var builders = services.AddIdentityServer()
                        .AddOperationalStore(options =>
                            options.ConfigureDbContext = builder =>
                                builder.UseSqlServer(databaseOptions.IdpConnectionString, sqlOptions => sqlOptions.MigrationsAssembly(databaseOptions.MigrationsAssembly))
                                )
                        .AddConfigurationStore(options =>
                            options.ConfigureDbContext = builder =>
                                builder.UseSqlServer(databaseOptions.IdpConnectionString, sqlOptions => sqlOptions.MigrationsAssembly(databaseOptions.MigrationsAssembly)))
                        .AddAspNetIdentity<ApplicationUser>();

                    services.AddSingleton<MigrationService>();
                });
        }
    }
}
