using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
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
                .ReturnsResponse(content, "application/json")
                .Verifiable();

            // Act
            var result = await _httpService.GetAsync<object>("test-endpoint");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenHttpRequestExceptionThrown()
        {
            // Arrange
            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-endpoint")
                .Throws<HttpRequestException>()
                .Verifiable();

            // Act
            var result = await _httpService.GetAsync<object>("test-endpoint");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenJsonReaderExceptionThrown()
        {
            // Arrange
            _httpHandlerMock.SetupRequest(HttpMethod.Get, $"{ApiBaseUrl}/test-endpoint")
                .Throws<JsonReaderException>()
                .Verifiable();

            // Act
            var result = await _httpService.GetAsync<object>("test-endpoint");

            // Assert
            Assert.Null(result);
        }
    }
}
