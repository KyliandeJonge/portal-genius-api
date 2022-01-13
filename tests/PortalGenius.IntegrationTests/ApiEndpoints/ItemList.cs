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
        public ItemDataResult itemData;

        public ItemList()
        {
            items = new Item[]
            {
                new Item { Id = Guid.NewGuid().ToString() },
                new Item { Id = Guid.NewGuid().ToString() },
                new Item { Id = Guid.NewGuid().ToString() },
            };

            itemData = new ItemDataResult
            {
                layers = new Layer[]
            {
                new Layer { Id = Guid.NewGuid().ToString() },
                new Layer { Id = Guid.NewGuid().ToString() },
                new Layer { Id = Guid.NewGuid().ToString() },
            }};
        }

      

        [Fact]
        public async Task GetAllItems_ShouldNotBeNull()
        {
            // Assert  
            var content = JsonConvert.SerializeObject(this.items);

            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitems")
                .ReturnsResponse(content, "application/json");

            // Act
            var result = await _httpService.GetAsync<Item[]>("test-get-allitems");


            // Arrange
            Assert.NotNull(this.items);
        }

        [Fact]
        public async Task GetAllItems_ReturnsAtleastOneItem()
        {
            // Assert
            var content = JsonConvert.SerializeObject(this.items);

            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitems")
                .ReturnsResponse(content, "application/json");

            // Act
            var result = await _httpService.GetAsync<Item[]>("test-get-allitems");


            // Arrange
            Assert.True(this.items.Length > 0);
        }

        [Fact]
        public async Task GetAllItemData_ShouldNotBeNull()
        {
            var content = JsonConvert.SerializeObject(this.itemData);
            // Assert
            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitemsparalell")
                .ReturnsResponse(content, "application/json");

            // Act
            var result = await _httpService.GetAsync<ItemDataResult>("test-get-allitemsparalell");

            // Arrange
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllItemData_ReturnsAtleastOneLayer()
        {
            var content = JsonConvert.SerializeObject(this.itemData);
            // Assert
            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitemsparalell")
                .ReturnsResponse(content, "application/json");

            // Act
            var result = await _httpService.GetAsync<ItemDataResult>("test-get-allitemsparalell");

            // Arrange
            Assert.True(result.layers.Length > 0);
        }

    }
}
