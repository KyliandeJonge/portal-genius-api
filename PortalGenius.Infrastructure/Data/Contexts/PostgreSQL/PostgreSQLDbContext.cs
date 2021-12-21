using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    public class PostgreSQLDbContext : AppDbContext
    {
        public PostgreSQLDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        public PostgreSQLDbContext(DbContextOptions<PostgreSQLDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));
        }
    }
}
