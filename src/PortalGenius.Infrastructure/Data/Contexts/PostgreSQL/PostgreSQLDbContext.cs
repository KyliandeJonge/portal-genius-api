using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortalGenius.Core.Models;

namespace PortalGenius.Infrastructure.Data
{
    /// <summary>
    ///     This <see cref="AppDbContext"/> implementation is responsible for the PostgreSQL connection.
    /// </summary>
    /// <remarks>
    ///     The tables must be configured in <see cref="AppDbContext"/>! Adding tables in this class may break the application.
    /// </remarks>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(e =>
            {
                e.Property(item => item.Created).HasConversion(
                    v => UnixTimeToDateTime(long.Parse(v)),
                    v => v.ToString()
                );
            });
        }

        private static DateTime UnixTimeToDateTime(long unixtime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime);

            return dtDateTime;
        }
    }
}
