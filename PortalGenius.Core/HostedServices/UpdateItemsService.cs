using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortalGenius.Core.Services;

namespace PortalGenius.Core.HostedServices
{
    public class UpdateItemsService : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly IArcGISService _arcGISService;

        private readonly ILogger<UpdateItemsService> _logger;

        public UpdateItemsService(
            IArcGISService arcGISService,
            ILogger<UpdateItemsService> logger
        )
        {
            _arcGISService = arcGISService;
            _logger = logger;
        }

        // Called on startup of the application
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync() at UpdateItemsService");

            // Start the callback immeadiatly (zero) and repeat every minute
            _timer = new Timer(UpdateItems, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        // Called on graceful shutdown of the application
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync() at UpdateItemsService");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void UpdateItems(object state)
        {
            _logger.LogInformation("UpdateItems() at UpdateItemsService");

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
