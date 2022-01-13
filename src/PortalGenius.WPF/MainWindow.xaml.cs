
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
        private readonly IArcGISService _arcGISService;

        private readonly IRepository<Item> _itemRepository;

        public MainWindow(
            IArcGISService arcGISService, 
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

        protected override async void OnClosing(CancelEventArgs e)
        {
            // MME 12-01-2022: voor deze test-app kan ik er mee leven hoor dat je de OnClosing en xxx_Click methods gebruikt
            // maar als je de code van het product zou zien, dan zul je zien dat ik dit niet doe.
            // voor user interfaces niet alleen wpf maar ook web dien je het mvvm-pattern te volgen (mvvm is voor wpf-applicatie, mvc voor web)
            // ik gebruik MVVMLightLibs voor vooral de messaging binnen MVVM
            // en ik gebruik MvvmCross voor de rest van het MVVM-pattern
            // zonder mvvm kun je je schermen niet unit testen

            // Stop the Kestrel host when this window closes
            await _host.StopAsync();
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

            _itemRepository.AddRange(items);
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
