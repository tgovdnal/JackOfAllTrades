using JackOfAllTTrades.Data.Postgres;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace JackOfAllTrade.Data.Migration
{
    public class Worker(IServiceProvider serviceProvider,IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {

        public const string ActivitySourceName = "Migrations";
        private static readonly ActivitySource s_activitySource = new(ActivitySourceName);



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PgContext>();

                await EnsureDatabaseExistsAsync(dbContext, stoppingToken);
                await MigrateDbAsync(dbContext, stoppingToken);
                await EnsureAdminUser(scope.ServiceProvider, dbContext, stoppingToken);

            }
            catch (Exception ex)
            {
                activity?.RecordException(ex);
                throw;
            }
            hostApplicationLifetime.StopApplication();
        }

        private static async Task EnsureDatabaseExistsAsync(PgContext pgContext, CancellationToken cancellationToken)
        {
            var creator = pgContext.GetService<IRelationalDatabaseCreator>();
            if(!await creator.ExistsAsync(cancellationToken))
                await creator.CreateAsync(cancellationToken);
        }

        private static async Task MigrateDbAsync( PgContext pgContext, CancellationToken cancellationToken)
        {
            await using var trans = await pgContext.Database.BeginTransactionAsync(cancellationToken);
            await pgContext.Database.MigrateAsync(cancellationToken);
            await trans.CommitAsync(cancellationToken);
        }
        
        private static async Task EnsureAdminUser(IServiceProvider serviceProvider,PgContext pgContext, CancellationToken cancellationToken)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var adminUser = await userManager.FindByNameAsync("Admin");

            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "IhrSicheresPasswort123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
        } 
    }
}
