using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortalGenius.Core.Services;

namespace PortalGenius.Core.HostedServices
{
    public class UpdateItemsService : BackgroundService
    {
        private readonly IArcGISService _arcGISService;

        private readonly IConfiguration _configuration;

        private readonly ILogger<UpdateItemsService> _logger;

        public UpdateItemsService(
            IArcGISService arcGISService,
            IConfiguration configuration,
            ILogger<UpdateItemsService> logger
        )
        {
            _arcGISService = arcGISService;
            _configuration = configuration;
            _logger = logger;
        }

        // Called on startup of the application
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync() at UpdateItemsService");

            while (!cancellationToken.IsCancellationRequested)
            {
                var test = await _arcGISService.GetAllItemsAsync();

                // TODO: Update database

                _logger.LogWarning("Updating database data");

                var item = test.Results.First();
                _logger.LogDebug("[{id}, {title}, {created}]", item.Id, item.Title, item.Created);

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
    }
}
