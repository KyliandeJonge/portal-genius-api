using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    public class SQLServerDbContext : AppDbContext
    {
        public SQLServerDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        public SQLServerDbContext(DbContextOptions<SQLServerDbContext> options, IConfiguration configuration) : base(options, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration.GetConnectionString("MSSQL"));
        }
    }
}
