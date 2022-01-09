using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Contrib.HttpClient;
using PortalGenius.Core.Services;
using System;
using System.Net;
using System.Net.Http;

namespace PortalGenius.UnitTests.Core.Services
{
    public class HttpServiceTests
    {
        protected readonly IHttpService _httpService;

        protected readonly Mock<HttpMessageHandler> _httpHandlerMock;

        protected const string ApiBaseUrl = "https://pg.arcgis-demo.dev/rest";

        public HttpServiceTests()
        {
            _httpHandlerMock = new Mock<HttpMessageHandler>();
            var factory = _httpHandlerMock.CreateClientFactory();

            var options = Options.Create(new HttpServiceOptions { HttpClientName = "test-client" });

            Mock.Get(factory).Setup(x => x.CreateClient(options.Value.HttpClientName))
                .Returns(() =>
                {
                    var client = _httpHandlerMock.CreateClient();
                    client.BaseAddress = new Uri(ApiBaseUrl);

                    return client;
                });

            // A simple example that returns 404 for any request
            _httpHandlerMock.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.NotFound);

            var logging = new Mock<ILogger<HttpService>>();

            _httpService = new HttpService(factory, options, logging.Object);
        }
    }
}
