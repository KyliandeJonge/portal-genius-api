using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PortalGenius.Core.Models;

namespace PortalGenius.Core.Services
{
    public class HttpService : IHttpService
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
        
        public async Task<T> GetAsync<T>(string path)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;

            var apiUrl = GenerateRequestUrl(path);

            try
            {
                var response = await _httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseContentRead);
                if (response.IsSuccessStatusCode)
                    result = await ParseHttpResponseToJsonAsync<T>(response);
                else
                    _logger.LogWarning("[HTTP GET {statusCode}] Something went wrong while connecting with: ({apiUrl}).", response.StatusCode, apiUrl);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("[HTTP GET 500] Error while connecting with: ({apiUrl}).", apiUrl);
                _logger.LogError(ex.Message);
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError("Error while parsing JSON");
                _logger.LogError(ex.Message);
            }
            return result;
        }

        public async Task<List<T>> GetSearchResultsAsync<T>(string path) where T : class
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            var result = new List<T>();

            var apiUrl = GenerateRequestUrl(path);
            var nextStart = -1;

            do
            {
                var nextUrl = (nextStart != -1) ? $"{apiUrl}&start={nextStart}" : apiUrl;

                var response = await _httpClient
                    .GetAsync(nextUrl, HttpCompletionOption.ResponseContentRead)
                    .ContinueWith(async (searchTask) =>
                    {
                        try
                        {
                            var response = await searchTask;
                            if (response.IsSuccessStatusCode)
                            {
                                var searchResult = await ParseHttpResponseToJsonAsync<SearchResult<T>>(response);

                                if (searchResult != null)
                                {
                                    // Build the full list to return later after the loop.
                                    if (searchResult.Results.Any())
                                        result.AddRange(searchResult.Results);

                                    // Get the URL for the next page
                                    nextStart = searchResult.NextStart;
                                }
                            }
                            else
                                _logger.LogWarning("[HTTP GET {statusCode}] Something went wrong while connecting with: ({apiUrl}).", response.StatusCode, apiUrl);
                        }
                        catch (HttpRequestException ex)
                        {
                            _logger.LogError("[HTTP GET 500] Error while connecting with: ({apiUrl}).", apiUrl);
                            _logger.LogError(ex.Message);
                        }
                        catch (JsonReaderException ex)
                        {
                            _logger.LogError("Error while parsing JSON");
                            _logger.LogError(ex.Message);
                        }
                    });

            } while (nextStart != -1);

            return result;
        }

        public async Task<T> PostAsync<T>(string path, StringContent stringContent)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;

            string apiUrl = GenerateRequestUrl(path);

            HttpResponseMessage response;    
            
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = stringContent,
                };
                requestMessage.Headers.Add("Cache-Control", "no-cache");

                response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                    result = await ParseHttpResponseToJsonAsync<T>(response);
                else
                    _logger.LogWarning("[HTTP POST {statusCode}] Something went wrong while connecting with: ({apiUrl}).", response.StatusCode, apiUrl);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("[HTTP POST 500] Error while connecting with: ({apiUrl}).", apiUrl);
                _logger.LogError(ex.Message);
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError("Error while parsing JSON");
                _logger.LogError(ex.Message);
            }

            return result;
        }
        
        /// <summary>
        ///     Converteer de JSON HTTP response naar de gewenste vorm.
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

        /// <summary>
        ///     Generate the absolute request URL to the API. This method trims the leading slash from the base address and
        ///     appends the slash manually to prevent double slashes which causes a 404 error.
        /// </summary>
        /// <param name="path">The relative path in the URL (e.g. rest/items/{itemId})</param>
        /// <returns>The correct absolute URL to call</returns>
        private string GenerateRequestUrl(string path) => $"{_httpClient.BaseAddress.ToString().Trim('/')}/{path}";
    }
}
