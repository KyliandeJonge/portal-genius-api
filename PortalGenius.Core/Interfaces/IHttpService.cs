namespace PortalGenius.Core.Services
{
    public interface IHttpService
    {
        public Task<T> GetAsync<T>(string path);

        public Task<T> PostAsync<T>(string path, object requestBody);
    }
}
