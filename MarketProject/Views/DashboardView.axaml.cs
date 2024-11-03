using System.Collections.ObjectModel;
using System.Linq;
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
        Database.SupplyList.CollectionChanged += (sender, _) =>
        {
            var newList = sender as ObservableCollection<Supply>;
            CardSupplyDashboard.CurrentSupply = newList!.FirstOrDefault(s => s.InDeliver);
        };
    }
}