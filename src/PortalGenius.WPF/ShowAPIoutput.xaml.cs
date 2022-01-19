using System.Windows;
using System.Windows.Controls;
using JSONTreeView;
using PortalGenius.Core.Services;

namespace PortalGenius.WPF;

/// <summary>
///     Interaction logic for ShowAPIoutput.xaml
/// </summary>
public partial class ShowAPIoutput : Page
{
    private readonly IArcGISService _arcGISService;

    public ShowAPIoutput(IArcGISService arcGISService)
    {
        _arcGISService = arcGISService;

        InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        //UserController U1 = new UserController();
        //ItemController itemcontroller = new ItemController();
        //Output.Text = itemcontroller.Post();

        // MME 12-01-2022: dit soort super efficiente code qua regel gebruik zou ik echt afraden
        // check out: https://wiki.c2.com/?TrainWreck dit is het train-wreck anti pattern
        // https://www.c-sharpcorner.com/article/train-wreck-pattern-cascade-method-pattern-in-C-Sharp/
        // heeft veel nadelen , 1: is dat je niet defensief kan programmeren, 2: de leesbaarheid is gewoon prut, 3: je hebt er totaal geen enkel voordeel bij in runtime

        Data.ProcessJson((await _arcGISService.GetAllItemsAsync()).ToString());
    }
}
