using JackOfAllTrade.Data.Migration;
using JackOfAllTTrades.Data.Postgres;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);


var pg = new RegisterPostgresService();
pg.RegisterServices(builder);

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<PgContext>();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
.WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var host = builder.Build();
host.Run();
