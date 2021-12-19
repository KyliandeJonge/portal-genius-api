using Microsoft.Extensions.DependencyInjection;

namespace PortalGenius.Core.Services
{
    public class HttpServiceOptions
    {
        /// <summary>
        /// The name of the <see cref="HttpClient"/> to use.
        /// </summary>
        public string HttpClientName { get; set; }
    }

    public static class HttpServiceCollectionExtentions
    {
        /// <summary>
        /// Adds the custom <see cref="HttpService"/> class to the service the service collection.
        /// This method accepts a <paramref name="clientName"/> to specify whichs <see cref="HttpClient"/> should be used in the <see cref="HttpService"/>.
        /// </summary>
        /// <param name="services">The service collection to add the <see cref="HttpService"/> to.</param>
        /// <param name="clientName">The name of the <see cref="HttpClient"/> to use.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddHttpService(this IServiceCollection services, string clientName)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (clientName is null) throw new ArgumentNullException(nameof(clientName));

            // Configure the custom HttpService class by storing the client name.
            // This name can be retreived using the IOptions interface inside the HttpService.
            services.Configure<HttpServiceOptions>((options) =>
            {
                options.HttpClientName = clientName;
            });

            // Add the HttpService to the service collection
            return services.AddTransient<HttpService>();
        }
    }
}
