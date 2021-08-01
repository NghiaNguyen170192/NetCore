using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Infrastructure.Database.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace NetCore.Api
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
                builder.UseSqlServer(databaseOptions.DatabaseConnection, options => 
                { 
                    options.MigrationsAssembly(databaseOptions.MigrationsAssembly); 
                });
            });

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

            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            AddSwagger(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCore Api V1.0.0");
            });

        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var assemblyName = ((ControllerActionDescriptor)apiDesc.ActionDescriptor).ControllerTypeInfo.Assembly.GetName().Name;
                    var currentAssemblyName = GetType().Assembly.GetName().Name;
                    return currentAssemblyName == assemblyName;
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NetCore Api",
                    Version = "v1.0.0",
                    Description = "NetCore Api"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //var xmlPath = Path.Combine(folderPath, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                c.DescribeAllParametersInCamelCase();
            });
        }

    }
}
