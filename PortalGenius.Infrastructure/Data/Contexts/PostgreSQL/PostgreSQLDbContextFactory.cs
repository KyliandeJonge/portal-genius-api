using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    public class PostgreSQLDbContextFactory : AppDbContextFactory<PostgreSQLDbContext>
    {
        public override DbContextOptionsBuilder<PostgreSQLDbContext> ConfigureDbContext(DbContextOptionsBuilder<PostgreSQLDbContext> builder, IConfigurationRoot configuration)
        {
            return builder.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
        }
    }
}
