namespace PortalGenius.Core.Services
{
    public interface IArcGISService
    {
        public Task<object> GetAllItemsAsync();

        public Task<object> GetAllUsersAsync();
    }
}
