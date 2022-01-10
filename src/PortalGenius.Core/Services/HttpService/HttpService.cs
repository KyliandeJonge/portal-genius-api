using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
        //o3VzBD0JKC
        public async Task<T> GetAsync<T>(string path)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;

            var apiUrl = $"{_httpClient.BaseAddress}/{path}";

            Console.WriteLine("Requesting: " + apiUrl);

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

        /*public async Task<T> PostAsync<T>(string path, object requestBody)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;

            var apiUrl = $"{_httpClient.BaseAddress}/{path}";

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, MediaTypeNames.Application.Json);

                var response = await _httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                    result = await ParseHttpResponseToJsonAsync<T>(response);
                else
                    _logger.LogWarning($"[HTTP POST {response.StatusCode}] Something went wrong while connecting with: ({apiUrl}).", response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"[HTTP POST 500] Error while connecting with: ({apiUrl}).");
                _logger.LogError(ex.Message);
            }
            
            return result;
        }*/

        public async Task<T> PostAsync<T>(string path, StringContent stringContent)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;
            string apiUrl = $"{_httpClient.BaseAddress}/{path}";
            HttpResponseMessage responseMessage;    
            
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = stringContent
                };

                requestMessage.Headers.Add("Cache-Control", "no-cache");

                responseMessage = await _httpClient.SendAsync(requestMessage);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("[HTTP POST 500] Error while connecting with: ({apiUrl}).", apiUrl);
                _logger.LogError(ex.Message);

                return result;
            }

            if (!responseMessage.IsSuccessStatusCode) return result;
            {
                try
                {
                    result = await ParseHttpResponseToJsonAsync<T>(responseMessage);
                }
                catch (JsonReaderException ex)
                {
                    _logger.LogError("Error while parsing JSON");
                    _logger.LogError(ex.Message);
                }
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
