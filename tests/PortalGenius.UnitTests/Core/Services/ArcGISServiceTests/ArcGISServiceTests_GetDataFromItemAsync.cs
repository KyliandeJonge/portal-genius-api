using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services
{
    public class ArcGISServiceTests_GetDataFromItemAsync : ArcGISServiceTests
    {
        public ArcGISServiceTests_GetDataFromItemAsync() : base()
        {
        }

        [Fact]
        public async Task GetDataFromItemAsync_ShouldReturnItemData_WhenItemsExists()
        {
            throw new NotImplementedException("No support for item data response yet");

            //// Arrange
            //var searchResults = new SearchResult<Item>
            //{
            //    Results = new Item[]
            //    {
            //        new Item { Id = Guid.NewGuid().ToString() },
            //        new Item { Id = Guid.NewGuid().ToString() },
            //        new Item { Id = Guid.NewGuid().ToString() },
            //    }
            //};

            //var item_id = It.IsAny<string>();
            //_httpHandlerMock.SetupRequest(HttpMethod.Get, r => r.RequestUri.AbsolutePath.StartsWith($"rest/content/items/{item_id}/data"))
            //    .ReturnsResponse(JsonConvert.SerializeObject(searchResults), "application/json");

            //// Act
            //var result = await _argGISService.GetAllItemsAsync();

            //// Assert
            //Assert.Equal(searchResults.Results.Length, result.Results.Length);
        }
    }
}
