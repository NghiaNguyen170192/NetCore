using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.AuthenticationDatabase;
using NetCore.Infrastructure.Data;
using NetCore.Infrastructure.Database;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Models.Identity;
using NetCore.Infrastructure.Services;

namespace NetCore.AuthServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseOptions = _configuration.GetDatabaseOptions("ConnectionStrings");
            //string connectionString = _configuration.GetConnectionString("DefaultConnection");
            //var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //var infrastructureMigrationAssembly = "NetCore.Infrastructure";

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(databaseOptions.IdpConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(databaseOptions.MigrationsAssembly);
                }));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
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

            builders.AddDeveloperSigningCredential();

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();

            SampleData.Migration(app);
            //SampleData.SeedUsersAndRoles(app);
            //SampleData.InitializeDbData(app);
        }
    }
}
