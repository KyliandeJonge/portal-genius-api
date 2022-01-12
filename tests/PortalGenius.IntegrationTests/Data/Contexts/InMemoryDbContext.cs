using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PortalGenius.Infrastructure.Data;

namespace PortalGenius.IntegrationTests.Data.Contexts
{
    public class InMemoryDbContext : AppDbContext
    {
        public InMemoryDbContext() : base(null)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            builder.UseInMemoryDatabase("portalgenius")
                   .UseInternalServiceProvider(serviceProvider);
        }
    }
}
