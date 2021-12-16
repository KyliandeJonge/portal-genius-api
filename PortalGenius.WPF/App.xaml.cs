using Microsoft.Extensions.DependencyInjection;
using PortalGenius.Core.Services;
using System;
using System.Windows;

namespace PortalGenius.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddHttpClient("local-api", options =>
            {
                options.BaseAddress = new Uri("https://portalgenius.maps.arcgis.com/sharing/rest");
            });

            services.AddTransient<ArcGISService>();
            services.AddHttpClient("local-api");

            // Pages
            services.AddTransient<ShowAPIoutput>();

            // Windows
            services.AddTransient<MainWindow>();
            services.AddTransient<APIWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();
        }
    }
}
