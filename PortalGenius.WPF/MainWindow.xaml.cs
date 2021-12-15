
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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

        public MainWindow()
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

            _host.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Stop the Kestrel host when this window closes
            _host.Dispose();

            base.OnClosing(e);
        }
    }
}
