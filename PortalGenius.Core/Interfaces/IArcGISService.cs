using PortalGenius.Core.Models;

namespace PortalGenius.Core.Services
{
    public interface IArcGISService
    {
        public Task<SearchResult<Item>> GetAllItemsAsync();

        public Task<object> GetDataFromItemAsync(string item_id);

        public Task<object> GetAllUsersAsync();

    }
}
