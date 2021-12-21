using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    public class OracleDbContext : AppDbContext
    {
        public OracleDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        public OracleDbContext(DbContextOptions<OracleDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(Configuration.GetConnectionString("Oracle"));
        }
    }
}
