using System.Net.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using PortalGenius.Core.Models;
using PortalGenius.Core.Services;

namespace PortalGenius.UnitTests.Core.Services;

public class ArcGISServiceTests : HttpServiceTests
{
    protected readonly IArcGISService _argGISService;

    public ArcGISServiceTests()
    {
        var logger = new Mock<ILogger<ArcGISService>>();

        // Mock the generate token URI
        var generateTokenContent = JsonConvert.SerializeObject(new GenerateToken { Token = "1234" });
        _httpHandlerMock.SetupRequest(HttpMethod.Post, $"{ApiBaseUrl}/generateToken")
            .ReturnsResponse(generateTokenContent, "application/json");

        _argGISService = new ArcGISService(_httpService, logger.Object);
    }
}
