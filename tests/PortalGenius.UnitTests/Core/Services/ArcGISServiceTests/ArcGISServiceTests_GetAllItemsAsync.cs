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
    public class ArcGISServiceTests_GetAllItemsAsync : ArcGISServiceTests
    {
        public ArcGISServiceTests_GetAllItemsAsync() : base()
        {
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnItemSearchResult_WhenOnePageAvailable()
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

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnAllItems_WhenMultiplePagesAvailable()
        {
            // Arrange
            var itemResponseOne = new SearchResult<Item>
            {
                Num = 3,
                // Mark the next index of first entry in the next resultset
                NextStart = 4,

                Results = new Item[]
                {
                    new Item { Id = Guid.NewGuid().ToString() },
                    new Item { Id = Guid.NewGuid().ToString() },
                    new Item { Id = Guid.NewGuid().ToString() },
                }
            };

            var itemResponseTwo = new SearchResult<Item>
            {
                Num = 3,
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
                .ReturnsResponse(JsonConvert.SerializeObject(itemResponseOne), "application/json");

            _httpHandlerMock.SetupRequest(HttpMethod.Get,
                    r => r.RequestUri.AbsolutePath.StartsWith("/rest/search") &&
                         r.RequestUri.PathAndQuery.Contains("start=4")
                )
                .ReturnsResponse(JsonConvert.SerializeObject(itemResponseTwo), "application/json");

            // Act
            var result = await _argGISService.GetAllItemsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), itemResponseOne.Results.Count() + itemResponseTwo.Results.Count());
        }
    }
}
