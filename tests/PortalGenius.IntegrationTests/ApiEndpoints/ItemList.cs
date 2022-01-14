using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using PortalGenius.UnitTests.Core.Services;
using Xunit;

namespace PortalGenius.IntegrationTests.ApiEndpoints;

[Collection("Sequential")]
public class ItemList : HttpServiceTests
{
    public ItemDataResult itemData;
    public Item[] items;

    public ItemList()
    {
        items = new[]
        {
            new() { Id = Guid.NewGuid().ToString() },
            new Item { Id = Guid.NewGuid().ToString() },
            new Item { Id = Guid.NewGuid().ToString() }
        };

        itemData = new ItemDataResult
        {
            layers = new[]
            {
                new() { Id = Guid.NewGuid().ToString() },
                new Layer { Id = Guid.NewGuid().ToString() },
                new Layer { Id = Guid.NewGuid().ToString() }
            }
        };
    }


    [Fact]
    public async Task GetAllItems_ShouldNotBeNull()
    {
        // Assert  
        var content = JsonConvert.SerializeObject(items);

        _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitems")
            .ReturnsResponse(content, "application/json");

        // Act
        var result = await _httpService.GetAsync<Item[]>("test-get-allitems");


        // Arrange
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllItems_ReturnsAtleastOneItem()
    {
        // Assert
        var content = JsonConvert.SerializeObject(items);

        _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitems")
            .ReturnsResponse(content, "application/json");

        // Act
        var result = await _httpService.GetAsync<Item[]>("test-get-allitems");


        // Arrange
        Assert.True(result.Length > 0);
    }

    [Fact]
    public async Task GetAllItemData_ShouldNotBeNull()
    {
        var content = JsonConvert.SerializeObject(itemData);
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
        var content = JsonConvert.SerializeObject(itemData);
        // Assert
        _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-get-allitemsparalell")
            .ReturnsResponse(content, "application/json");

        // Act
        var result = await _httpService.GetAsync<ItemDataResult>("test-get-allitemsparalell");

        // Arrange
        Assert.True(result.layers.Length > 0);
    }
}
