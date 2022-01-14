using PortalGenius.Core.Models;
using PortalGenius.Infrastructure.Data;
using PortalGenius.IntegrationTests.Data.Contexts;

namespace PortalGenius.IntegrationTests.Data
{
    /// <summary>
    ///     This fixture creates an in-memory Application Database Context and provides a repository for the <see cref="Item"/> table to use as an example.
    /// </summary>
    /// <remarks>
    ///     Example from <a href="https://github.com/ardalis/CleanArchitecture/blob/main/tests/Clean.Architecture.IntegrationTests/Data/BaseEfRepoTestFixture.cs">Ardalis Clean Architecture</a>
    /// </remarks>
    public abstract class BaseEfRepoTestFixture
    {
        protected AppDbContext _dbContext;

        public BaseEfRepoTestFixture()
        {
            _dbContext = new InMemoryDbContext();
        }

        protected Repository<Item> GetRepository()
        {
            return new Repository<Item>(_dbContext);
        }
    }
}
