
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortalGenius.Core.Interfaces;
using PortalGenius.Core.Models;
using PortalGenius.Core.Services;
using PortalGenius.Infrastructure.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PortalGenius.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IHost _host;
        private readonly ArcGISService _arcGISService;

        private readonly IRepository<Item> _itemRepository;

        public MainWindow(
            ArcGISService arcGISService, 
            IRepository<Item> itemRepository
        )
        {
            InitializeComponent();
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost.UseStartup<ApiStartup>();
                    webHost.UseKestrel();

                    // Webhost URL's is configured via appsettings.{env}.json
                })
                .Build();

            _arcGISService = arcGISService;
            _itemRepository = itemRepository;

            _host.Start();

            // Finish loading component
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Stop the Kestrel host when this window closes
            _host.Dispose();
            base.OnClosing(e);
        }

        /// <summary>
        /// Retrieves Arcgis items from our api and saves them into the Sqlite database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGetItemsAndInsertInDatabase_Click(object sender, RoutedEventArgs e)
        {
            
            var items = await _arcGISService.GetAllItemsAsync();

            _itemRepository.AddRange(items.Results);
            await _itemRepository.SaveChangesAsync();
            dgMainDg.ItemsSource = await _itemRepository.GetAllAsync();

            btnGetItemsAndInsertInDatabase.IsEnabled = false;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgMainDg.ItemsSource = await _itemRepository.GetAllAsync();
        }
    }
}
