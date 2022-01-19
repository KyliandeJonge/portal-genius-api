using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services;

public class ArcGISServiceTests_GetAllItemsAsync : ArcGISServiceTests
{
    [Fact]
    public async Task GetAllItemsAsync_ShouldReturnItemSearchResult_WhenItemsAvailable()
    {
        // Arrange
        var searchResults = new SearchResult<Item>
        {
            // Mark no new entries beyond first resultset
            NextStart = -1,
            Results = new[]
            {
                new() { Id = Guid.NewGuid().ToString() },
                new Item { Id = Guid.NewGuid().ToString() },
                new Item { Id = Guid.NewGuid().ToString() }
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
