using log4net;
using Newtonsoft.Json;
using PortalGenius.Core.Models;

namespace PG_API.Services
{
    public class ArcGISService
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<ArcGISService> _logger;

        public ArcGISService(
            IHttpClientFactory httpClientFactory,
            ILogger<ArcGISService> logger
        )
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("arcgis-api");
        }

        public async Task<object> GetItems()
        {
            _logger.LogInformation("Dit is met Log4J??");

            // TODO: Make accountId dynamic
            return await GetRequest<object>("search?q=accountid:v16XTZeIhHAZEpwh&f=json");
        }

        public async Task<object> GetUsers()
        {
            return await GetRequest<object>("portals/x/users?f=json&token=6Jv9FkkWq7T78yD4Egh_2ZIDhv39DWlMBN_ps49ibz0EBihF8pJ7hgrO6Ru_yjGRXWoT9IKAKGEwikddYyBlTfLXo-zYk0eW1EVqdgR7MI2LGtLoRg8YNoNaHp01kCRoVfbAmh6Xm_6IJQcz2le647fvR9FXwtb7EQ-SRwAz2Zbf6xwPohkF6lBjhcoEoPdTHw-6X5iwKADpJEKOtb2fwQ..&searchUserAccess=*&filter=*&num=100");
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
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message, ex);
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

