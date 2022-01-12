using PortalGenius.Core.Models;
using System;
using System.Linq;
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

            // MME 12-01-2022: leesbaarheid wordt beter als je dit gewoon verdeeld over 2 regels
            // daarnaast geeft het meer mogelijkheden om defensief te programmeren
            var newItem = (await repository.GetAllAsync()).FirstOrDefault();

            // Arrange
            Assert.Equal(testId, newItem?.Id);
        }
    }
}
