using JackOfAllTTrades.Data.Postgres;

namespace JackOfAllTrade.Data.Migration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            var pg = new RegisterPostgresService();
            pg.RegisterServices(builder);
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

            var host = builder.Build();
            host.Run();
        }
    }
}