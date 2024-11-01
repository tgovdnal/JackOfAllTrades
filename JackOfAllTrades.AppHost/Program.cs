using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres")
                        .WithDataVolume("jack-data",false)
                        .WithPgAdmin();
var postgresdb = postgres.AddDatabase($"{JackOfAllTrades.Common.Constants.DBName}-db");

//var apiService = builder.AddProject<Projects.JackOfAllTrades_ApiService>("apiservice");

builder.AddProject<Projects.JackOfAllTrades_Web>("web")
    .WithExternalHttpEndpoints()
    .WithReference(cache);

builder.Build().Run();
