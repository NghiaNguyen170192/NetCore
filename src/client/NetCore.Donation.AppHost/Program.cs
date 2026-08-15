using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume();

var appDb = postgres.AddDatabase("netcore-donation-db");

var redis = builder.AddRedis("redis");

var migrator = builder.AddProject<Projects.NetCore_Donation_Migration>("migration")
    .WithReference(appDb)
    .WithReference(redis)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("Database__Provider", "postgresql")
    .WithEnvironment("Database__MigrationsAssembly", "NetCore.Donation.Infrastructure.Database")
    .WithEnvironment("Database__ApplicationConnectionString", appDb)
    .WithArgs("-m", "-s")
    .ExcludeFromManifest();

var api = builder.AddProject<Projects.NetCore_Donation_Api>("api")
    .WithReference(appDb)
    .WithReference(redis)
    .WaitFor(migrator)
    .WithEnvironment("Database__Provider", "postgresql")
    .WithEnvironment("Database__MigrationsAssembly", "NetCore.Donation.Infrastructure.Database")
    .WithEnvironment("Database__ApplicationConnectionString", appDb)
    .WithHttpEndpoint(port: 6000, name: "api-http")
    .WithHttpsEndpoint(port: 6001, name: "api-https");

var ui = builder.AddProject<Projects.NetCore_Donation_UI>("ui")
    .WithReference(api)
    .WithEnvironment("ApiBaseAddress", () => api.GetEndpoint("api-http").Url)
    .WithHttpEndpoint(port: 6010, name: "ui-http")
    .WithHttpsEndpoint(port: 6011, name: "ui-https");

await builder.Build().RunAsync();