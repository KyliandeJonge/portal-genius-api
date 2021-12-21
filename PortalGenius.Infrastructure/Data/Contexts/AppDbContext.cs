using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortalGenius.Core.Models;

namespace PortalGenius.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // SOURCE: https://dev.to/moesmp/ef-core-multiple-database-providers-3gb7

        protected readonly IConfiguration Configuration;

        #region Tables

        public DbSet<Item> Items => Set<Item>();

        #endregion  

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }
    }
}
