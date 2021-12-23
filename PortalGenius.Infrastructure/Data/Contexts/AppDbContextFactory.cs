using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    /// <summary>
    ///     This class is used to run Entity Framework commands locally (design-time).
    /// </summary>
    /// <remarks>
    ///     During runtime, the configurations from the Startup class are being used.
    /// </remarks>
    /// <typeparam name="T">An implementation of <see cref="AppDbContext"/>.</typeparam>
    public abstract class AppDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : AppDbContext
    {
        public T CreateDbContext(string[] args)
        {
            // Get the application configurations from the startup-project.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")

                // Optionally use the environment-specific appsettings
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            // Configure the context to use the correct database service (e.g. UseSqlServer)
            // This is determined by the corresponding IDesignTimeDbContextFactory 
            var builder = new DbContextOptionsBuilder<T>();
            ConfigureDbContext(builder, configuration);

            // Create an instance of the correct DbContext to use and return it
            return (T)Activator.CreateInstance(typeof(T), builder.Options, configuration);
        }

        /// <summary>
        ///     Configure the implementation of <see cref="AppDbContext"/> for design-time usage.
        ///     Usualy this only means that the right database-implementation and the connectionstring is configured. 
        /// </summary>
        /// <param name="builder">The optionsbuilder to apply the configuration to.</param>
        /// <param name="root">A reference to the application configuration properties to read the connectionstrings for example.</param>
        /// <returns></returns>
        public abstract DbContextOptionsBuilder<T> ConfigureDbContext(DbContextOptionsBuilder<T> builder, IConfigurationRoot root);
    }
}
