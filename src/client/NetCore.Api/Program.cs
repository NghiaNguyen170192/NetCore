using NetCore.Application.Extensions;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Override URLs if not set by Aspire
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    builder.WebHost.UseUrls("http://localhost:6000", "https://localhost:6001");
}

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();

builder.Services.AddProblemDetails();

// Dependency Injections
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.AddLogger("netcore-api");

var app = builder.Build();

app.UseDefaultOpenApi();
app.MapDefaultEndpoints();

await app.RunAsync();