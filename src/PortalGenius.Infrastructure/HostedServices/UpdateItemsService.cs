using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortalGenius.Core.Interfaces;
using PortalGenius.Core.Models;
using PortalGenius.Core.Services;

namespace PortalGenius.Infrastructure.HostedServices
{
    /// <summary>
    ///     This background service is responsible for refreshing the ArcGIS data in the database.
    ///     The interval time can be configured using the <c>DataRefreshInterval</c> property in appsettings.json.
    /// </summary>
    /// <remarks>
    ///     As soon as the application starts, this background service starts.
    /// </remarks>
    public class UpdateItemsService : BackgroundService
    {
        private readonly IArcGISService _arcGISService;

        private readonly IConfiguration _configuration;

        private readonly IServiceProvider _service;

        private readonly ILogger<UpdateItemsService> _logger;

        public UpdateItemsService(
            IArcGISService arcGISService,
            IConfiguration configuration,
            IServiceProvider service,
            ILogger<UpdateItemsService> logger
        )
        {
            _arcGISService = arcGISService;
            _configuration = configuration;
            _service = service;
            _logger = logger;
        }

        // Called on startup of the application
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (! cancellationToken.IsCancellationRequested)
            {
                await UpdateItemsAsync();

                // Get the interval value from the configuration and delay this task
                if (TimeSpan.TryParse(_configuration["DataRefreshInterval"], out TimeSpan interval))
                    await Task.Delay(interval, cancellationToken);
                else
                {
                    _logger.LogError("Invalid configuration value '{interval}' for property 'DataRefreshInterval'", _configuration["DataRefreshInterval"]);
                    break;
                }
            }
        }

        private async Task UpdateItemsAsync()
        {
            // Fetch all items from ArcGIS
            var itemSearchResults = await _arcGISService.GetAllItemsAsync();

            // Background services need their own scope to use services
            using (var scope = _service.CreateScope())
            {
                // Work via the Item repository
                var repo = scope.ServiceProvider.GetRequiredService<IRepository<Item>>();

                // Remove the old items first
                _logger.LogWarning("REMOVING existing data");
                repo.RemoveRange(await repo.GetAllAsync());

                // Bulk insert the latest items.
                _logger.LogWarning("Updating database data");
                repo.AddRange(itemSearchResults.Results);

                // Mutate the changes
                await repo.SaveChangesAsync();
            }

            _logger.LogInformation("{count} item(s) inserted into the database", itemSearchResults.Results.Count());
        }
    }
}
