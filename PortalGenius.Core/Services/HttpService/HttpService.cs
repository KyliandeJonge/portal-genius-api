﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace PortalGenius.Core.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<HttpService> _logger;

        public HttpService(
            IHttpClientFactory httpClientFactory,
            IOptions<HttpServiceOptions> httpServiceOptions,
            ILogger<HttpService> logger
        )
        {
            _httpClient = httpClientFactory.CreateClient(httpServiceOptions.Value.HttpClientName);
            _logger = logger;
        }

        public async Task<T> GetRequest<T>(string path)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;

            var apiUrl = $"{_httpClient.BaseAddress}/{path}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseContentRead);
                if (response.IsSuccessStatusCode)
                    result = await ParseHttpResponseToJsonAsync<T>(response);
                else
                    _logger.LogWarning($"[HTTP {response.StatusCode}] Something went wrong while connecting with: ({apiUrl}).", response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"[HTTP 500] Error while connecting with: ({apiUrl}).");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Converteer de JSON HTTP response naar de gewenste vorm.
        /// </summary>
        /// <typeparam name="T">De vorm waar de JSON naartoe geconverteerd moet worden.</typeparam>
        /// <param name="response">De HTTP-response message.</param>
        /// <returns>De geserialiseerde JSON in de gewenste vorm.</returns>
        private async Task<T> ParseHttpResponseToJsonAsync<T>(HttpResponseMessage response)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            return new JsonSerializer().Deserialize<T>(jsonReader);
        }
    }
}