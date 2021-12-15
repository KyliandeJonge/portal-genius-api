using log4net;
using Newtonsoft.Json;
using PortalGenius.Core.Services;

namespace PG_API.Services
{
    public class ArcGISService
    {
        private readonly HttpClient _httpClient;

        private readonly ILog _logger = LogManager.GetLogger(typeof(ArcGISService));

        public ArcGISService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("arcgis-api");
        }

        public async Task<List<object>> GetItems()
        {
            return await GetRequest<List<object>>("items");
        }

        public async Task<T> GetRequest<T>(string path)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T? result = default;

            var apiUrl = $"{_httpClient.BaseAddress}/{path}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseContentRead);
                if (response.IsSuccessStatusCode)
                    result = await ParseHttpResponseToJsonAsync<T>(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex.Message, ex);
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

