﻿using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using NetCore.Infrastructure.AuthenticationDatabase;
using NetCore.Infrastructure.AuthenticationDatabase.Models;
using NetCore.Infrastructure.Data;
using NetCore.Infrastructure.Services;
using NetCore.Shared.Configurations;

namespace NetCore.IdentityProvider
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly DatabaseConfiguration _databaseConfiguration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            IdentityModelEventSource.ShowPII = true;
            _configuration = configuration;
            _environment = environment;

            _databaseConfiguration = new DatabaseConfiguration();
            _configuration.GetSection("Database").Bind(_databaseConfiguration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(_databaseConfiguration.IdpConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(_databaseConfiguration.MigrationsAssembly);
                }));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builders = services.AddIdentityServer()
                .AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(_databaseConfiguration.IdpConnectionString, sqlOptions => sqlOptions.MigrationsAssembly(_databaseConfiguration.MigrationsAssembly))
                        )
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(_databaseConfiguration.IdpConnectionString, sqlOptions => sqlOptions.MigrationsAssembly(_databaseConfiguration.MigrationsAssembly)))
                .AddAspNetIdentity<ApplicationUser>();

            if (_environment.IsDevelopment())
            {
                builders.AddDeveloperSigningCredential();
            }

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            RunMigration(app);
        }

        private void RunMigration(IApplicationBuilder app)
        {
            SampleData.Migration(app);
            //SampleData.SeedUsersAndRoles(app);
            SampleData.InitializeDbData(app, "secret");
        }
    }
}
