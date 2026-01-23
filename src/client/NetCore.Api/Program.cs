using NetCore.ServiceDefaults;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.AppSettingConfigurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();

var databaseConfiguration = new DatabaseConfiguration();
builder.Configuration.GetSection("Database").Bind(databaseConfiguration);

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