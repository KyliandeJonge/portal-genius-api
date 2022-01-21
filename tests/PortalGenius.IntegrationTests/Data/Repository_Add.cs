using PortalGenius.Core.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.IntegrationTests.Data
{
    public class Repository_Add : BaseEfRepoTestFixture
    {
        [Fact]
        public async Task Add_AddsAndSavesItem()
        {
            // Assert
            var testId = Guid.NewGuid().ToString();
            var repository = GetRepository();
            var item = new Item { Id = testId };

            // Act
            repository.Add(item);
            await repository.SaveChangesAsync();

            var newItem = await repository.GetFirstOrDefaultAsync();

            // Arrange
            Assert.Equal(testId, newItem?.Id);
        }
    }
}
