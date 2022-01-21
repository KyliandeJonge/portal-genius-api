
using Dasync.Collections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using PortalGenius.Core.Data;
using PortalGenius.Core.Interfaces;
using PortalGenius.Core.Models;
using PortalGenius.Core.Services;
using PortalGenius.Infrastructure.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            btnClearDatabase.IsEnabled = true;
        }

        private async void btnClearDatabase_Click(object sender, RoutedEventArgs e)
        {
            _itemRepository.RemoveRange(await _itemRepository.GetAllAsync());
            await _itemRepository.SaveChangesAsync();
            dgMainDg.ItemsSource = null;

            btnGetItemsAndInsertInDatabase.IsEnabled = true;
            btnClearDatabase.IsEnabled = false;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgMainDg.ItemsSource = await _itemRepository.GetAllAsync();
        }

        private async void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();


            if (saveFileDialog.ShowDialog() == true)
            {
                //string ID = ((Item)dgMainDg.SelectedItems[0]).Id;

                var items = await _arcGISService.GetAllItemsAsync();

                var dirname = Path.GetDirectoryName(saveFileDialog.FileName);

                await items.ParallelForEachAsync(item =>
                {
                    string URI = $"https://portalgenius.maps.arcgis.com/sharing/rest/content/items/{item.Id}/data?f=json&token={UserData.genToken}";
                    var path = $"{dirname}\\{item.Name}";
                    using (WebClient wc = new())
                    {
                        wc.DownloadFileAsync(
                                // Param1 = Link of file
                                new System.Uri(URI),
                                // Param2 = Path to save
                                $"C:\\Users\\Matthijs\\Downloads\\Items\\{item.Name}"
                            );
                    }

                    return Task.CompletedTask;
                });
            }
        }
    }
}
