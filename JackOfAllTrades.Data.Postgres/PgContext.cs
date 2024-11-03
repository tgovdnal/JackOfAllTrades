using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JackOfAllTTrades.Data.Postgres
{

    public class PgContext : IdentityDbContext
    {
        
        public DbSet<PgPost> Posts => Set<PgPost>();
        public PgContext(DbContextOptions<PgContext> options) : base(options)
        {
        }

    }
}
