using NetCore.ServiceDefaults;
using NetCore.Infrastructure.Database.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDefaultOpenApi();
builder.Services.AddHealthChecks();
builder.AddApplicationServices();

builder.Host.AddLogger("netcore-api");

var app = builder.Build();

app.UseDefaultOpenApi();
app.MapDefaultEndpoints();

await app.RunAsync();