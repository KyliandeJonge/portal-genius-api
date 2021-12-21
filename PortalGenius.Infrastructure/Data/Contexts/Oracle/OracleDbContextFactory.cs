using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    public class OracleDbContextFactory : AppDbContextFactory<OracleDbContext>
    {
        public override DbContextOptionsBuilder<OracleDbContext> UseDatabaseService(DbContextOptionsBuilder<OracleDbContext> builder, IConfigurationRoot configuration)
        {
            return builder.UseOracle(configuration.GetConnectionString("Oracle"));
        }
    }
}
