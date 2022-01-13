using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using PortalGenius.UnitTests.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.IntegrationTests.ApiEndpoints
{
    [Collection("Sequential")]
    public class ItemList : HttpServiceTests
    {
        public Item[] items;
        public ItemList () 
        {
            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitems")
                .ReturnsResponse("test", "application/json");

            items = new Item[]
                {
                    new Item { Id = Guid.NewGuid().ToString() },
                    new Item { Id = Guid.NewGuid().ToString() },
                    new Item { Id = Guid.NewGuid().ToString() },
                };
            }

        [Fact]
        public async Task GetAllItems_ReturnsOKStatus()
        {
            // Assert
            await using var application = new ApiApplication();
            var content = JsonConvert.SerializeObject(this.items);

            var client = application.CreateClient();
            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitems")
                .ReturnsResponse(content, "application/json");

            // Act
            var result = await _httpService.GetAsync<Item[]>("test-get-allitems");


            // Arrange
            Assert.Equal(this.items, result);
        }

        [Fact]
        public async Task GetAllItems_ReturnsAtleastOneItem()
        {
            // Assert
            await using var application = new ApiApplication();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/");
            var items = await ApiApplication.ParseHttpResponseToJsonAsync<List<Item>>(response);
            
            // Arrange
            Assert.True(items.Count > 0);
            Assert.True(items.Count() > 0);
        }

        [Fact]
        public async Task GetAllItemDataParallel_IsNotNull()
        {
            // Assert
            await using var application = new ApiApplication();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/allDataParallel");
            var data = await ApiApplication.ParseHttpResponseToJsonAsync<List<object>>(response);
           
            // Arrange
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetAllItemDataParallel_ReturnsAtleastOneItemData()
        {
            // Assert
            await using var application = new ApiApplication();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/allDataParallel");
            var data = await ApiApplication.ParseHttpResponseToJsonAsync<List<object>>(response);

            // Arrange
            Assert.True(data.Count > 0);
        }

        [Fact]
        public async Task GetAllItemDataSequential_IsNotNull()
        {
            // Assert
            await using var application = new ApiApplication();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/allDataSequential");
            var data = await ApiApplication.ParseHttpResponseToJsonAsync<List<object>>(response);

            // Arrange
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetAllItemDataSequential_ReturnsAtleastOneItemData()
        {
            // Assert
            await using var application = new ApiApplication();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/allDataSequential");
            var data = await ApiApplication.ParseHttpResponseToJsonAsync<List<object>>(response);

            // Arrange
            Assert.True(data.Count > 0);
        }
    }
}
