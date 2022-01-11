using PortalGenius.Core.Models;

namespace PortalGenius.Core.Services
{
    public interface IArcGISService
    {
        public Task<SearchResult<Item>> GetAllItemsAsync();

        public Task<object> GetDataFromItemAsync(string item_id);

        public Task<object> GetAllUsersAsync();

        public Task<GenerateToken> GetGenToken(string username = "", string password = "", bool newCreds = false);

        public Task<GenerateToken> GetGenTokenWithNewCreds(string username, string password);
    }
}
