﻿using Microsoft.Extensions.Logging;
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

            // Configure the HttpClient for each test
            // A factory is required as the HttpService injects a Client Factory
            Mock.Get(factory)

                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(() =>
                {
                    var client = _httpHandlerMock.CreateClient();
                    client.BaseAddress = new Uri(ApiBaseUrl);

                    return client;
                });

            // Return a 404 resonse for each request by default
            _httpHandlerMock.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.NotFound);

            // The actual HttpClientName is not relevant for unit-testing in this case.
            var options = Options.Create(new HttpServiceOptions());
            var logging = new Mock<ILogger<HttpService>>();

            _httpService = new HttpService(factory, options, logging.Object);
        }
    }
}
