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

                _logger.LogWarning("Updating database data");
                _logger.LogDebug(test.Results.First().Type);

                try
                {
                    // Get the interval value from the configuration and delay this task
                    var interval = TimeSpan.Parse(_configuration["DataRefreshInterval"]);

                    await Task.Delay(interval, cancellationToken);
                }
                catch (Exception)
                {
                    _logger.LogError("Invalid configuration value '{0}' for property 'DataRefreshInterval'", _configuration["DataRefreshInterval"]);
                }
            }
        }
    }
}
