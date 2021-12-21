using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortalGenius.Core.Models;

namespace PortalGenius.Infrastructure.Data
{
    /// <summary>
    ///     This <see cref="DbContext"/> implementation acts as the 'base' database context and must be used to talk with the database (no matter which type).
    ///     Each databasetype has its own context implementation whichs derives from <see cref="AppDbContext"/> to support migrations for multiple database types.
    /// </summary>
    /// <remarks>
    ///     The tables must be configured in this class to be used in each <see cref="AppDbContext"/> implemantation!
    /// </remarks>
    public abstract class AppDbContext : DbContext
    {
        // SOURCE: https://dev.to/moesmp/ef-core-multiple-database-providers-3gb7

        protected readonly IConfiguration Configuration;

        #region Tables

        public DbSet<Item> Items => Set<Item>();

        #endregion  

        /// <summary>
        ///     Construct the base database context without additional <see cref="DbContextOptions"/>
        ///     when registering the this <see cref="DbContext"/> implementation as a service in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="configuration">The available application configuration properties to be used, amongst others, for the <see cref="DbContextOptionsBuilder"/>.</param>
        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///     Constuct the base database context with the option to provide additional <see cref="DbContextOptions"/> 
        ///     when registering the this <see cref="DbContext"/> implementation as a service in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="configuration">The available application configuration properties to be used, amongst others, for the <see cref="DbContextOptionsBuilder"/>.</param>
        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }
    }
}
