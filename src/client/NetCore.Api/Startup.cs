using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.Infrastructure.Database;
using NetCore.Shared.Configurations;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace NetCore.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly DatabaseOptions _databaseOptions;
        private readonly AuthenticationServerConfiguration _authenticationServerConfiguration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            IdentityModelEventSource.ShowPII = true;
            _configuration = configuration;
            _environment = environment;

            _databaseOptions = new DatabaseOptions();
            _configuration.GetSection("ConnectionStrings").Bind(_databaseOptions);

            _authenticationServerConfiguration = new AuthenticationServerConfiguration();
            _configuration.GetSection("AuthenticationServer").Bind(_authenticationServerConfiguration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(builder =>
            {
                builder.UseSqlServer(_databaseOptions.ApplicationConnectionString, options =>
                {
                    options.MigrationsAssembly(_databaseOptions.MigrationsAssembly);
                });
            });

            services.AddAuthorization();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    //destination of authen server
                    options.Authority = _authenticationServerConfiguration.Issuer;
                    //destination of web api
                    options.Audience = _authenticationServerConfiguration.Audience;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = GetTokenValidationParameters();
                });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();

            services.AddMediatR(typeof(Infrastructure.Database.Handlers.AssemblyReference).GetTypeInfo().Assembly);

            if (_environment.IsDevelopment())
            {
                services.AddSwaggerGen(options =>
                {
                    options.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        var assemblyName = ((ControllerActionDescriptor)apiDesc.ActionDescriptor).ControllerTypeInfo.Assembly.GetName().Name;
                        var currentAssemblyName = GetType().Assembly.GetName().Name;
                        return currentAssemblyName == assemblyName;
                    });

                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "NetCore Api",
                        Version = "v1.0.0",
                        Description = "NetCore Api"
                    });

                    #region swagger for JWT Bearer authentication
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                    #endregion

                    options.DescribeAllParametersInCamelCase();
                });
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCore Api V1.0.0");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _authenticationServerConfiguration.Issuer,
                ValidateAudience = true,
                ValidAudience = _authenticationServerConfiguration.Audience,
                //ValidateIssuerSigningKey = true,
                //IssuerSigningKey = GetSecurityKey(),
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
        }

        private SecurityKey GetSecurityKey()
        {
            X509Certificate2 certificate = GetCertificate();
            return new X509SecurityKey(certificate);
        }

        private X509Certificate2 GetCertificate()
        {
            string fileName = _authenticationServerConfiguration.CertificatePath;
            string password = _authenticationServerConfiguration.CertificatePassword;
            var certificate = new X509Certificate2(fileName, password, X509KeyStorageFlags.MachineKeySet);
            return certificate;
        }
    }
}