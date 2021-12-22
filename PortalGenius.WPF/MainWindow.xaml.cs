
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PortalGenius.Core.Services;
using PortalGenius.Infrastructure.Data;
using System.ComponentModel;
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
        private readonly AppDbContext _appDbContext;

        public MainWindow(ArcGISService arcGISService)
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
            //_appDbContext = appDbContext;

            _host.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Stop the Kestrel host when this window closes
            _host.Dispose();

            base.OnClosing(e);
        }

        private async void btnGetItemsAndInsertInDatabase_Click(object sender, RoutedEventArgs e)
        {
            var items = await _arcGISService.GetAllItems();
            //JObject joItems = JObject.Parse(items.ToString());
            //this.ArcgisItems = JsonConvert.DeserializeObject<List<Item>>((string)joItems["results"].ToString());

            _appDbContext.AddRange(items);
            _appDbContext.SaveChanges();
        }
    }
}
