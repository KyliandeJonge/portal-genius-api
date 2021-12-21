using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    public class SQLiteDbContext : AppDbContext
    {
        public SQLiteDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite(Configuration.GetConnectionString("Sqlite"));
        }
    }
}
