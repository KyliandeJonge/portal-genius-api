using PG_API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortalGenius.WPF
{
    /// <summary>
    /// Interaction logic for ShowAPIoutput.xaml
    /// </summary>
    public partial class ShowAPIoutput : Page
    {
        public ShowAPIoutput()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserController U1 = new UserController();
            ItemController itemcontroller = new ItemController();
            Output.Text = itemcontroller.Post();
        }
    }
}
