using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services;

public class HttpServiceTests_GetAsync : HttpServiceTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnData_WhenResponseIsSuccess()
    {
        // Arrange
        var itemResponse = new Item { Id = Guid.NewGuid().ToString() };
        var content = JsonConvert.SerializeObject(itemResponse);

        _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-endpoint")
            .ReturnsResponse(content, "application/json");

        // Act
        var result = await _httpService.GetAsync<Item>("test-endpoint");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, itemResponse.Id);
    }

    [Fact]
    public async Task GetAsync_ShouldLogWarning_WhenHttpResponseIsError()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";
        var statusCode = HttpStatusCode.BadRequest;

        _httpHandlerMock.SetupRequest(HttpMethod.Get, endpoint)
            .ReturnsResponse(statusCode);

        // Act
        var result = await _httpService.GetAsync<object>("test-endpoint");

        // Assert
        _httpServiceLogger.VerifyLog(logger =>
            logger.LogWarning("[HTTP GET {statusCode}] Something went wrong while connecting with: ({apiUrl}).",
                statusCode, endpoint));
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenHttpResponseIsError()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";
        var statusCode = HttpStatusCode.BadRequest;

        _httpHandlerMock.SetupRequest(HttpMethod.Get, endpoint)
            .ReturnsResponse(statusCode);

        // Act
        var result = await _httpService.GetAsync<object>("test-endpoint");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenHttpRequestExceptionThrown()
    {
        // Arrange
        _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-endpoint")
            .Throws<HttpRequestException>();

        // Act
        var result = await _httpService.GetAsync<object>("test-endpoint");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldLogError_WhenHttpRequestExceptionThrown()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";

        _httpHandlerMock.SetupRequest(HttpMethod.Get, endpoint)
            .Throws<HttpRequestException>();

        // Act
        var result = await _httpService.GetAsync<object>("test-endpoint");

        // Assert
        _httpServiceLogger.VerifyLog(logger =>
            logger.LogError("[HTTP GET 500] Error while connecting with: ({apiUrl}).", endpoint));
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenJsonReaderExceptionThrown()
    {
        // Arrange
        _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-endpoint")
            .Throws<JsonReaderException>();

        // Act
        var result = await _httpService.GetAsync<object>("test-endpoint");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldLogError_WhenJsonReaderExceptionThrown()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";

        _httpHandlerMock.SetupRequest(HttpMethod.Get, endpoint)
            .Throws<JsonReaderException>();

        // Act
        var result = await _httpService.GetAsync<object>("test-endpoint");

        // Assert
        _httpServiceLogger.VerifyLog(logger => logger.LogError("Error while parsing JSON"));
    }
}
