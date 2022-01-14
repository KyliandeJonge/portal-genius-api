using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data;

public class SQLServerDbContextFactory : AppDbContextFactory<SQLServerDbContext>
{
    public override DbContextOptionsBuilder<SQLServerDbContext> ConfigureDbContext(
        DbContextOptionsBuilder<SQLServerDbContext> builder, IConfigurationRoot configuration)
    {
        return builder.UseSqlServer(configuration.GetConnectionString("MSSQL"));
    }
}
