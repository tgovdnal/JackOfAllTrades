var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.JackOfAllTrades_ApiService>("apiservice");

builder.AddProject<Projects.JackOfAllTrades_Web>("web")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
