using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.AppSettingConfigurations;
using NetCore.Infrastructure.Database.Extensions;

var builder = WebApplication.CreateBuilder(args);
var databaseConfiguration = new DatabaseConfiguration();
builder.Configuration.GetSection("Database").Bind(databaseConfiguration);
var modelBuilder = new ODataConventionModelBuilder();

builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen(options =>
	{
		options.DescribeAllParametersInCamelCase();
		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			In = ParameterLocation.Header,
			Description = "Please enter 'Bearer' following by space and JWT",
			Name = "Authorization",
			Type = SecuritySchemeType.ApiKey
		});
		options.AddSecurityRequirement(new OpenApiSecurityRequirement{
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
	})
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers()
	.AddOData(options =>
	{
		options.Filter().Expand()
			.Select().OrderBy().SetMaxTop(100).SkipToken()
			.AddRouteComponents("odata", modelBuilder.GetEdmModel());
		options.EnableNoDollarQueryOptions = true;
	});

builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "AllowAll",
		policy =>
		{
			policy.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
		});
});

//Dependency Injections
builder.Services
	.AddApplication()
	.AddInfrastructure(databaseConfiguration);

builder.Host.AddLogger("netcore-api");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseRouting();
app.UseCors("AllowAll");
//app.UseAuthorization();
app.UseHealthChecks("/healthz");
app.MapControllers();

await app.RunAsync();