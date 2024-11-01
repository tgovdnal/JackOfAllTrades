using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
