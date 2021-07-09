using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Shared;

namespace NetCore.API
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

            //services.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseSqlServer(connectionString));

            //services.AddAuthorization();
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        //destination of authen server
            //        options.Authority = "http://localhost:52646/";
            //        //destination of web api
            //        options.Audience = "api1";
            //        options.RequireHttpsMetadata = false;
            //    });

            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
