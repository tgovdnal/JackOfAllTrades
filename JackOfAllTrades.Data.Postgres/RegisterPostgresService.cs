using JackOfAllTrades.Abstractions;
using JackOfAllTrades.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackOfAllTTrades.Data.Postgres
{
    public class RegisterPostgresService : IRegisterServices
    {

        public IHostApplicationBuilder RegisterServices(IHostApplicationBuilder host)
        {
            host.Services.AddScoped<IPostRepository, PgPostRepository>();
            host.AddNpgsqlDbContext<PgContext>(Constants.DBName);
            return host;
        }
    }
}
