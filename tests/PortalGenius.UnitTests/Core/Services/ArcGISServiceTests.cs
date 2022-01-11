using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using PortalGenius.Core.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services
{
    public class ArcGISServiceTests : HttpServiceTests
    {
        private readonly IArcGISService _argGISService;

        public ArcGISServiceTests() : base()
        {
            var logger = new Mock<ILogger<ArcGISService>>();

            // Mock the generate token URI
            var generateTokenContent = JsonConvert.SerializeObject(new GenerateToken { Token = "1234" });
            _httpHandlerMock.SetupRequest(HttpMethod.Post, $"{ApiBaseUrl}/generateToken")
                .ReturnsResponse(generateTokenContent, "application/json");

            _argGISService = new ArcGISService(base._httpService, logger.Object);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnItemSearchResult_WhenItemsAvailable()
        {
            // Arrange
            var searchResults = new SearchResult<Item>
            {
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
            Assert.Equal(searchResults.Results.Length, result.Results.Length);
        }
    }
}
