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

public class HttpServiceTests_PostAsync : HttpServiceTests
{
    [Fact]
    public async Task PostAsync_ShouldReturnData_WhenResponseIsSuccess()
    {
        // Arrange
        var itemRequestBody = new Item { Id = Guid.NewGuid().ToString() };
        var content = JsonConvert.SerializeObject(itemRequestBody);

        _httpHandlerMock.SetupRequest(HttpMethod.Post, $"{ApiBaseUrl}/test-endpoint")
            .ReturnsResponse(content, "application/json")
            .Verifiable();

        // Act
        var result = await _httpService.PostAsync<Item>("test-endpoint", new StringContent(content));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, itemRequestBody.Id);
    }

    [Fact]
    public async Task PostAsync_ShouldLogWarning_WhenHttpResponseIsError()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";
        var statusCode = HttpStatusCode.BadRequest;

        var requestBody = new { Test = "Name" };
        var content = JsonConvert.SerializeObject(requestBody);

        _httpHandlerMock.SetupRequest(HttpMethod.Post, endpoint)
            .ReturnsResponse(statusCode);

        // Act
        var result = await _httpService.PostAsync<object>("test-endpoint", new StringContent(content));

        // Assert
        _httpServiceLogger.VerifyLog(logger =>
            logger.LogWarning("[HTTP POST {statusCode}] Something went wrong while connecting with: ({apiUrl}).",
                statusCode, endpoint));
    }

    [Fact]
    public async Task PostAsync_ShouldReturnNull_WhenHttpResponseIsError()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";
        var statusCode = HttpStatusCode.BadRequest;

        var requestBody = new { Test = "Name" };
        var content = JsonConvert.SerializeObject(requestBody);

        _httpHandlerMock.SetupRequest(HttpMethod.Post, endpoint)
            .ReturnsResponse(statusCode);

        // Act
        var result = await _httpService.PostAsync<object>("test-endpoint", new StringContent(content));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PostAsync_ShouldReturnNull_WhenHttpRequestExceptionThrown()
    {
        // Arrange
        var requestBody = new { Test = "Name" };
        var content = JsonConvert.SerializeObject(requestBody);

        _httpHandlerMock.SetupRequest(HttpMethod.Post, $"{ApiBaseUrl}/test-endpoint")
            .Throws<HttpRequestException>();

        // Act
        var result = await _httpService.PostAsync<object>("test-endpoint", new StringContent(content));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PostAsync_ShouldLogError_WhenHttpRequestExceptionThrown()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";

        var requestBody = new { Test = "Name" };
        var content = JsonConvert.SerializeObject(requestBody);

        _httpHandlerMock.SetupRequest(HttpMethod.Post, endpoint)
            .Throws<HttpRequestException>();

        // Act
        var result = await _httpService.PostAsync<object>("test-endpoint", new StringContent(content));

        // Assert
        _httpServiceLogger.VerifyLog(logger =>
            logger.LogError("[HTTP POST 500] Error while connecting with: ({apiUrl}).", endpoint));
    }

    [Fact]
    public async Task PostAsync_ShouldReturnNull_WhenJsonReaderExceptionThrown()
    {
        // Arrange
        var requestBody = new { Test = "Name" };
        var content = JsonConvert.SerializeObject(requestBody);

        _httpHandlerMock.SetupRequest(HttpMethod.Post, $"{ApiBaseUrl}/test-endpoint")
            .Throws<JsonReaderException>();

        // Act
        var result = await _httpService.PostAsync<object>("test-endpoint", new StringContent(content));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PostAsync_ShouldLogError_WhenJsonReaderExceptionThrown()
    {
        // Arrange
        var endpoint = $"{ApiBaseUrl}/test-endpoint";

        var responseBody = new { Test = "Name" };
        var content = JsonConvert.SerializeObject(responseBody);

        _httpHandlerMock.SetupRequest(HttpMethod.Post, endpoint)
            .Throws<JsonReaderException>();

        // Act
        var result = await _httpService.PostAsync<object>("test-endpoint", new StringContent(content));

        // Assert
        _httpServiceLogger.VerifyLog(logger => logger.LogError("Error while parsing JSON"));
    }
}
