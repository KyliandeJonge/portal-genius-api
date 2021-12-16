using PortalGenius.Core.Services;
using System.Windows;
using System.Windows.Controls;

namespace PortalGenius.WPF
{
    /// <summary>
    /// Interaction logic for ShowAPIoutput.xaml
    /// </summary>
    public partial class ShowAPIoutput : Page
    {
        private readonly ArcGISService _arcGISService;

        public ShowAPIoutput(ArcGISService arcGISService)
        {
            _arcGISService = arcGISService;

            InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //UserController U1 = new UserController();
            //ItemController itemcontroller = new ItemController();
            //Output.Text = itemcontroller.Post();

            Output.Text = (await _arcGISService.GetAllItems()).ToString();
        }
    }
}
