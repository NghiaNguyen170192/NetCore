using NetCore.ServiceDefaults;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.AppSettingConfigurations;

var builder = WebApplication.CreateBuilder(args);
var databaseConfiguration = new DatabaseConfiguration();
builder.Configuration.GetSection("Database").Bind(databaseConfiguration);

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();

builder.Services.AddProblemDetails();

//Dependency Injections
builder.Services
    .AddApplication()
    .AddInfrastructure(databaseConfiguration);

builder.Host.AddLogger("netcore-api");

var app = builder.Build();

app.UseDefaultOpenApi();
app.MapDefaultEndpoints();

await app.RunAsync();