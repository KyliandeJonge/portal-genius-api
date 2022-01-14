using System;
using System.Threading.Tasks;
using PortalGenius.Core.Models;
using Xunit;

namespace PortalGenius.IntegrationTests.Data;

public class Repository_Remove : BaseEfRepoTestFixture
{
    [Fact]
    public async Task Remove_ReturnsAllItemsExceptRemoved()
    {
        // Assert
        var repository = GetRepository();

        var itemToRemove = new Item { Id = "remove" };
        var items = new[]
        {
            new() { Id = Guid.NewGuid().ToString() },
            itemToRemove
        };

        repository.AddRange(items);
        await repository.SaveChangesAsync();

        // Act & Arrange
        Assert.Contains(await repository.GetAllAsync(), item => item.Id == itemToRemove.Id);

        repository.Remove(itemToRemove);
        await repository.SaveChangesAsync();

        Assert.DoesNotContain(await repository.GetAllAsync(), item => item.Id == itemToRemove.Id);
    }
}
