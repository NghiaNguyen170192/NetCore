using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Shared;

namespace NetCore.Tools.Migration
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

            services.AddDbContext<DatabaseContext>(builder =>
            {
                builder.UseNpgsql(databaseOptions.DatabaseConnection, o => o.MigrationsAssembly(databaseOptions.MigrationsAssembly));
            });

            services.AddHostedService<MigrationService>();
        }
    }
}
