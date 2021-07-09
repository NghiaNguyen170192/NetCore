using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            //string connectionString = _configuration.GetConnectionString("DefaultConnection");
            //var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //var infrastructureMigrationAssembly = "NetCore.Infrastructure";

            //services.AddDbContext<ApplicationDbContext>(builder =>
            //    builder.UseSqlServer(connectionString, sqlOptions =>
            //    {
            //        sqlOptions.MigrationsAssembly(infrastructureMigrationAssembly);
            //        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            //    }));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //var builders = services.AddIdentityServer()
            //    .AddOperationalStore(options =>
            //        options.ConfigureDbContext = builder =>
            //            builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(infrastructureMigrationAssembly))
            //            )
            //    .AddConfigurationStore(options =>
            //        options.ConfigureDbContext = builder =>
            //            builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(infrastructureMigrationAssembly)))
            //    .AddAspNetIdentity<ApplicationUser>();

            //builders.AddDeveloperSigningCredential();

            //services.AddTransient<IProfileService, IdentityClaimsProfileService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            //app.UseIdentityServer();

            //SampleData.Migration(app);
            //SampleData.SeedUsersAndRoles(app);
            //SampleData.InitializeDbData(app);
        }
    }
}
