using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PortalGenius.Core.Services;
using PortalGenius.Infrastructure.Data;
using System;
using System.Windows;

namespace PortalGenius.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext, SQLiteDbContext>();

            services.AddHttpClient("local-api", options =>
            {
                options.BaseAddress = new Uri("https://portalgenius.maps.arcgis.com/sharing/rest");
            });

            services.AddHttpService("local-api");
            services.AddTransient<ArcGISService>();

            // Windows
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
