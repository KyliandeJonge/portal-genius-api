using Microsoft.Extensions.DependencyInjection;

namespace PortalGenius.Core.Services
{
    public class HttpServiceOptions
    {
        public string HttpClientName { get; set; }
    }

    public static class HttpServiceCollectionExtentions
    {
        public static IServiceCollection AddHttpService(this IServiceCollection services, string clientName)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (clientName == null) throw new ArgumentNullException(nameof(clientName));

            services.Configure<HttpServiceOptions>((options) =>
            {
                options.HttpClientName = clientName;
            });

            return services.AddTransient<HttpService>();
        }
    }
}
