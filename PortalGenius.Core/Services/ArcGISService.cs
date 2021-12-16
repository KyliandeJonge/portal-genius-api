using Microsoft.Extensions.Logging;

namespace PortalGenius.Core.Services
{
    public class ArcGISService
    {
        private readonly HttpService _httpService;

        private readonly ILogger<ArcGISService> _logger;

        public ArcGISService(
            HttpService httpService,
            ILogger<ArcGISService> logger
        )
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<object> GetItems()
        {
            // TODO: Make accountId dynamic
            return await _httpService.GetRequest<object>("search?q=accountid:v16XTZeIhHAZEpwh&f=json");
        }
    }
}

