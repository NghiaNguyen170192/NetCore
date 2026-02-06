using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", secret: true);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume();

var appDb = postgres.AddDatabase("netcore-db");
var idpDb = postgres.AddDatabase("netcore-idp-db");

var redis = builder.AddRedis("redis");

var migrator = builder.AddProject<Projects.NetCore_Migration>("migration")
    .WithReference(appDb)
    .WithReference(idpDb)
    .WithReference(redis)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("Database__Provider", "postgresql")
    .WithEnvironment("Database__MigrationsAssembly", "NetCore.Infrastructure.Database")
    .WithEnvironment("Database__ApplicationConnectionString", appDb)
    .WithEnvironment("Database__IdpConnectionString", idpDb)
    .WithArgs("-d", "-m", "-s", "-t");

var api = builder.AddProject<Projects.NetCore_Api>("api")
    .WithReference(appDb)
    .WithReference(redis)
    .WithReference(migrator)
    .WithEnvironment("Database__Provider", "postgresql")
    .WithEnvironment("Database__MigrationsAssembly", "NetCore.Infrastructure.Database")
    .WithEnvironment("Database__ApplicationConnectionString", appDb)
    .WithEnvironment("Database__IdpConnectionString", idpDb)
    .WithHttpEndpoint(port: 6000, targetPort: 80, name: "api-http")
    .WithHttpsEndpoint(port: 6001, targetPort: 443, name: "api-https");

var ui = builder.AddProject<Projects.NetCore_UI>("ui")
    .WithReference(api)
    .WithEnvironment("ApiBaseAddress", () => api.GetEndpoint("api-http").Url)
    .WithHttpEndpoint(port: 6010, targetPort: 80, name: "ui-http")
    .WithHttpsEndpoint(port: 6011, targetPort: 443, name: "ui-https");

builder.Build().Run();