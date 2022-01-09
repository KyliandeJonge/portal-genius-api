using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PortalGenius.UnitTests.Core.Services
{
    public class HttpServiceTests_PostAsync : HttpServiceTests
    {
        public HttpServiceTests_PostAsync() : base()
        {
        }

        [Fact]
        public async Task PostAsync_ShouldYYYYYY_WhenZZZZZ()
        {
            // Arrange
            var responseBody = new { Test = "Name" };
            var content = JsonConvert.SerializeObject(responseBody);

            _httpHandlerMock.SetupRequest(HttpMethod.Post, $"{ApiBaseUrl}/test-endpoint")
                .ReturnsResponse(content, "application/json")
                .Verifiable();

            // Act
            var result = await _httpService.PostAsync<object>("test-endpoint", new StringContent(content));

            // Assert
            Assert.NotNull(result);
        }

    }
}
