using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    public class SQLiteDbContextFactory : AppDbContextFactory<SQLiteDbContext>
    {
        public override DbContextOptionsBuilder<SQLiteDbContext> ConfigureDbContext(DbContextOptionsBuilder<SQLiteDbContext> builder, IConfigurationRoot configuration)
        {
            return builder.UseSqlite(configuration.GetConnectionString("Sqlite"));
        }
    }
}
