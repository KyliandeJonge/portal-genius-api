using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data;

/// <summary>
///     This <see cref="AppDbContext" /> implementation is responsible for the Oracle database connection.
/// </summary>
/// <remarks>
///     The tables must be configured in <see cref="AppDbContext" />! Adding tables in this class may break the
///     application.
/// </remarks>
public class OracleDbContext : AppDbContext
{
    public OracleDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    public OracleDbContext(DbContextOptions<OracleDbContext> options, IConfiguration configuration) : base(options,
        configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(Configuration.GetConnectionString("Oracle"));
    }
}
