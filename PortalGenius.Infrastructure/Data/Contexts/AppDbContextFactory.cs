using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PortalGenius.Infrastructure.Data
{
    /// <summary>
    /// This class is used to run Entity Framework commands locally (design-time).
    /// For runtime, the configuration in the Startup class is used!
    /// </summary>
    public abstract class AppDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : AppDbContext
    {
        public T CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")

                // Optionally use the environment-specific appsettings
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<T>();

            // Configure the context to use the correct database service (e.g. UseSqlServer)
            // This is determined by the corresponding IDesignTimeDbContextFactory 
            UseDatabaseService(builder, configuration);

            // Create an instance of the correct DbContext to use
            var dbContext = (T)Activator.CreateInstance(typeof(T), builder.Options, configuration);

            return dbContext;
        }

        public abstract DbContextOptionsBuilder<T> UseDatabaseService(DbContextOptionsBuilder<T> builder, IConfigurationRoot root);
    }
}
