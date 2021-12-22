using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortalGenius.Core.Services;
using PortalGenius.Infrastructure.Data;
using System;
using System.IO;
using System.Windows;

namespace PortalGenius.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }

        public IConfiguration Configuration { get; set; }

        public App()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            InitializeConfiguration(services);

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

        private void InitializeConfiguration(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")

                // Optionally use the environment-specific appsettings
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            services.AddScoped(_ => Configuration);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
