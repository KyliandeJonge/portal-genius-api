using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services
{
    public class HttpServiceTests_GetAsync : HttpServiceTests
    {
        public HttpServiceTests_GetAsync() : base()
        {
        }

        [Fact]
        public async Task GetAsync_ShouldReturnData_WhenResponseIsSuccess()
        {
            // Arrange
            var responseBody = new { Test = "Name" };
            var content = JsonConvert.SerializeObject(responseBody);

            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-endpoint")
                .ReturnsResponse(content, "application/json");

            // Act
            var result = await _httpService.GetAsync<object>("test-endpoint");

            // Assert
            Assert.NotNull(result);
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
            _httpServiceLogger.VerifyLog(logger => logger.LogWarning("[HTTP GET {statusCode}] Something went wrong while connecting with: ({apiUrl}).", statusCode, endpoint));
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
            _httpServiceLogger.VerifyLog(logger => logger.LogError("[HTTP GET 500] Error while connecting with: ({apiUrl}).", endpoint));
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
}
