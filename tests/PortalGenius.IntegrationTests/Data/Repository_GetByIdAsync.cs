using PortalGenius.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.IntegrationTests.Data
{
    public class Repository_GetByIdAsync : BaseEfRepoTestFixture
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectItem()
        {
            // Assert
            var expectedId = "123";
            var expectedTitle = "Expected item";

            var repository = GetRepository();
            var items = new Item[]
            {
                new Item { Id = Guid.NewGuid().ToString() },
                new Item { Id = expectedId, Title = expectedTitle },
                new Item { Id = Guid.NewGuid().ToString() },
            };

            repository.AddRange(items);
            await repository.SaveChangesAsync();

            // Act
            var item = await repository.GetByIdAsync("123");

            // Arrange
            Assert.Equal(expectedId, item.Id);
            Assert.Equal(expectedTitle, item.Title);
        }
    }
}
