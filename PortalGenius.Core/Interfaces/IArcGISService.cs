using PortalGenius.Core.Models;

namespace PortalGenius.Core.Services
{
    public interface IArcGISService
    {
        public Task<SearchResult<Item>> GetAllItemsAsync();

        public Task<object> GetAllUsersAsync();

        public Task<object> GetDataFromItem(string item_id);
    }
}
