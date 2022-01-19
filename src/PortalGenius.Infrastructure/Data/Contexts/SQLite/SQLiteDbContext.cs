using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data;

/// <summary>
///     This <see cref="AppDbContext" /> implementation is responsible for the SQLite connection.
/// </summary>
/// <remarks>
///     The tables must be configured in <see cref="AppDbContext" />! Adding tables in this class may break the
///     application.
/// </remarks>
public class SQLiteDbContext : AppDbContext
{
    public SQLiteDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options, IConfiguration configuration) : base(options,
        configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite(Configuration.GetConnectionString("Sqlite"));
    }
}
