namespace PortalGenius.Core.Services
{
    public interface IHttpService
    {
        /// <summary>
        ///     Exectute a GET request and parse the JSON response to <typeparamref name="T"/> when the response is valid.
        ///     When the response is invalid, the path does not exist or when any other error occurs, null is returned.
        /// </summary>
        /// <typeparam name="T">The type to which the JSON response should be deserialized to.</typeparam>
        /// <param name="path">The path (without base address) to call.</param>
        /// <returns>The deserialized response on status OK.</returns>
        public Task<T> GetAsync<T>(string path);

        /// <summary>
        ///     Exectute a GET request and parse the JSON search result response to a <see cref="List"/> of <typeparamref name="T"/> when the response is valid.
        ///     When the response is invalid, the path does not exist or when any other error occurs, null is returned.
        /// </summary>
        /// <remarks>
        ///     This method only applies to the ArcGIS Api response!
        /// </remarks>
        /// <typeparam name="T">The type to which the JSON response should be deserialized to.</typeparam>
        /// <param name="path">The path (without base address) to call.</param>
        /// <returns>The deserialized response on status OK.</returns>
        public Task<List<T>> GetSearchResultsAsync<T>(string path) where T : class;

        /// <summary>
        ///     Exectute a POST request and parse the JSON response to <typeparamref name="T"/> when the response is valid.
        ///     When the response is invalid, the path does not exist or when any other error occurs, null is returned.
        /// </summary>
        /// <typeparam name="T">The type to which the JSON response should be deserialized to.</typeparam>
        /// <param name="path">The path (without base address) to call.</param>
        /// <param name="stringContent">Query parameters</param>
        /// <returns>The deserialized response on status OK.</returns>
        public Task<T> PostAsync<T>(string path, StringContent stringContent);
    }
}
