var builder = DistributedApplication.CreateBuilder(args);

var netcoreApi = builder.AddProject<Projects.NetCore_Api>("netcore-api");

await builder.Build().RunAsync();