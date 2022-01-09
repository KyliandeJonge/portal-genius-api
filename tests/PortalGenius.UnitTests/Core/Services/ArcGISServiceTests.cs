using Microsoft.Extensions.Logging;
using Moq;
using PortalGenius.Core.Models;
using PortalGenius.Core.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services
{
    public class ArcGISServiceTests
    {
        private readonly IArcGISService _argGISService;
        private readonly Mock<IHttpService> _httpServiceMock = new();

        public ArcGISServiceTests()
        {
            var httpClient = new Mock<IHttpService>();
            var logger = new Mock<ILogger<ArcGISService>>();

            _argGISService = new ArcGISService(httpClient.Object, logger.Object);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnItemSearchResult_WhenItemsAvailable()
        {
            // Arrange
            var searchResults = new SearchResult<Item>
            {
                Results = new Item[]
                {
                    new Item { Id = Guid.NewGuid().ToString() }
                }
            };

            _httpServiceMock
                .Setup(x => x.GetAsync<SearchResult<Item>>("items"))
                .ReturnsAsync(searchResults);

            // Act
            var result = await _argGISService.GetAllItemsAsync();

            // Assert
            Assert.Equal(searchResults.Results.Length, result.Results.Length);
        }
    }
}
