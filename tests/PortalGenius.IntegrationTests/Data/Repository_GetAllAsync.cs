using System;
using System.Linq;
using System.Threading.Tasks;
using PortalGenius.Core.Models;
using Xunit;

namespace PortalGenius.IntegrationTests.Data;

public class Repository_GetAllAsync : BaseEfRepoTestFixture
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllItem()
    {
        // Assert
        var repository = GetRepository();
        var items = new[]
        {
            new() { Id = Guid.NewGuid().ToString() },
            new Item { Id = Guid.NewGuid().ToString() },
            new Item { Id = Guid.NewGuid().ToString() }
        };

        repository.AddRange(items);
        await repository.SaveChangesAsync();

        // Act
        var allItems = await repository.GetAllAsync();

        // Arrange
        Assert.Equal(items.Length, allItems?.Count());
    }
}
