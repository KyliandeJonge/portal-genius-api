using Microsoft.Extensions.Logging;

namespace PortalGenius.Core.Services
{
    public class ArcGISService : IArcGISService
    {
        private readonly IHttpService _httpService;

        private readonly ILogger<ArcGISService> _logger;

        public ArcGISService(
            IHttpService httpService,
            ILogger<ArcGISService> logger
        )
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<object> GetAllItemsAsync()
        {
            // TODO: Make accountId dynamic
            return await _httpService.GetAsync<object>("search?q=accountid:v16XTZeIhHAZEpwh&f=json");
        }

        public async Task<object> GetAllUsersAsync()
        {
            return await _httpService.GetAsync<object>("portals/x/users?f=json&token=6Jv9FkkWq7T78yD4Egh_2ZIDhv39DWlMBN_ps49ibz0EBihF8pJ7hgrO6Ru_yjGRXWoT9IKAKGEwikddYyBlTfLXo-zYk0eW1EVqdgR7MI2LGtLoRg8YNoNaHp01kCRoVfbAmh6Xm_6IJQcz2le647fvR9FXwtb7EQ-SRwAz2Zbf6xwPohkF6lBjhcoEoPdTHw-6X5iwKADpJEKOtb2fwQ..&searchUserAccess=*&filter=*&num=100");
        }
    }
}

