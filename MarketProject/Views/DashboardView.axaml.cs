using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MarketProject.Models;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class DashboardView : UserControl
{
   // private DashboardViewModel _vm => DataContext as DashboardViewModel;
    public DashboardView()
    {
        InitializeComponent();
       // CardSupplyDashboard.CurrentSupply = _vm.CurrentSupply;
    }
}