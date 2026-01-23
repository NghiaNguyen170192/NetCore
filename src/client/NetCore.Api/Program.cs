using NetCore.ServiceDefaults;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.AppSettingConfigurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();

builder.Services.AddProblemDetails();

//Dependency Injections
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.AddLogger("netcore-api");

var app = builder.Build();

app.UseDefaultOpenApi();
app.MapDefaultEndpoints();

await app.RunAsync();