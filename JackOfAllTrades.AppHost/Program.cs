

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres")
                        .WithDataVolume("jack-data",false)
                        .WithPgAdmin();
var postgresdb = postgres.AddDatabase(JackOfAllTrades.Common.Constants.DBName);
var migration = builder.AddProject<Projects.JackOfAllTrades_Data_Migration>($"{JackOfAllTrades.Common.Constants.DBName}-migration")
    .WithReference(postgresdb);

//var apiService = builder.AddProject<Projects.JackOfAllTrades_ApiService>("apiservice");

builder.AddProject<Projects.JackOfAllTrades_Web>("web")
    .WithExternalHttpEndpoints()
    .WithReference(cache);

builder.Build().Run();
