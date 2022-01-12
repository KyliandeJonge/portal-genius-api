using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services
{
    public class ArcGISServiceTests_GetGenTokenAsync : ArcGISServiceTests
    {
        public ArcGISServiceTests_GetGenTokenAsync() : base()
        {
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnItemSearchResult_WhenItemsAvailable()
        {
            // Arrange
            var searchResults = new SearchResult<Item>
            {
                // Mark no new entries beyond first resultset
                NextStart = -1,
                Results = new Item[]
                {
                    new Item { Id = Guid.NewGuid().ToString() },
                    new Item { Id = Guid.NewGuid().ToString() },
                    new Item { Id = Guid.NewGuid().ToString() },
                }
            };

            _httpHandlerMock.SetupRequest(HttpMethod.Get, r => r.RequestUri.AbsolutePath.StartsWith("/rest/search"))
                .ReturnsResponse(JsonConvert.SerializeObject(searchResults), "application/json");

            // Act
            var result = await _argGISService.GetAllItemsAsync();

            // Assert
            Assert.Equal(searchResults.Results.Length, result.Count());
        }
    }
}
