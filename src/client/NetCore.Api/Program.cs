using NetCore.Application.Extensions;
using NetCore.Domain.SharedKernel;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Database.Services;
using NetCore.Infrastructure.Database.Middleware;
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

// Add correlation ID support
builder.Services.AddScoped<ICorrelationIdAccessor>(sp => 
    new CorrelationIdAccessor(sp.GetRequiredService<IHttpContextAccessor>()));

// Dependency Injections
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.AddLogger("netcore-api");

var app = builder.Build();

app.UseDefaultOpenApi();

// Use correlation ID middleware early in pipeline
app.UseCorrelationId();

app.MapDefaultEndpoints();

await app.RunAsync();