using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    /// <summary>
    ///     This <see cref="AppDbContext"/> implementation is responsible for the Microsoft SQL Server connection.
    /// </summary>
    /// <remarks>
    ///     The tables must be configured in <see cref="AppDbContext"/>! Adding tables in this class may break the application.
    /// </remarks>
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
