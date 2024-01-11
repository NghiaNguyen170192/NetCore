var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedisContainer("redis");
var mssql = builder.AddSqlServerContainer("mssql");

var netcoreApi = builder
    .AddProject<Projects.NetCore_Api>("netcore-api")
    .WithReference(mssql);

await builder.Build().RunAsync();