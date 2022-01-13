using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using PortalGenius.Infrastructure.Data;
using PortalGenius.UnitTests.Core.Services;
using Xunit;

namespace PortalGenius.IntegrationTests.ApiEndpoints
{
    [Collection("Sequential")]
    public class ItemList : HttpServiceTests
    {
        public Item[] items;
        public ItemList () {
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
            var response = await client.GetAsync("test-get-allitems");
            var result = await _httpService.GetAsync<object>("test-get-allitems");


            // Arrange
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
<<<<<<< HEAD
            Assert.True(items.Count > 0);
=======
            Assert.True(items.Count() > 0);
>>>>>>> ad96a55a64e07e6e100664c88530f95eafee5c13
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
