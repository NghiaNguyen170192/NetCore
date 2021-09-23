using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Infrastructure.Database.Extensions;
using System.Collections.Generic;
using System.Linq;
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

            services
                .AddControllers()
                .AddOData(options =>
                {
                    options.Select().Filter().Expand().OrderBy().Count().SetMaxTop(250);
                })
                .AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddMediatR(typeof(Infrastructure.Handlers.AssemblyReference).GetTypeInfo().Assembly);

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

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //var xmlPath = Path.Combine(folderPath, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                options.DescribeAllParametersInCamelCase();
            });

            AddODataFormattersForSwagger(services);
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseHttpsRedirection();
            //app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCore Api V1.0.0");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
        }
        
        private void AddODataFormattersForSwagger(IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                var outputFormatters = options
                        .OutputFormatters.OfType<ODataOutputFormatter>()
                        .Where(foramtter => foramtter.SupportedMediaTypes.Count == 0);

                foreach (var outputFormatter in outputFormatters)
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/odata"));
                }
            });
        }
    }
}
