using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using DynamicData;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
        Database.SupplyList.CollectionChanged += (sender, _) =>
        {
            var newList = sender as ObservableCollection<Supply>;
            CardSupplyDashboard.CurrentSupply = newList!.FirstOrDefault(s => s.InDeliver);
        };

        Database.FoodsMenuList.CollectionChanged += (_, _) =>
        {
            FoodCardsStackPanel.Children.Clear();
            foreach (var foods in Database.FoodsMenuList)
                FoodCardsStackPanel.Children.Add(new FoodDashboardCards { CurrentFood = foods});
        };
        
        if (Database.FoodsMenuList.Count == 0) return;

        foreach (var foods in Database.FoodsMenuList.TakeLast(3))
            FoodCardsStackPanel.Children.Add(new FoodDashboardCards { CurrentFood = foods});
    }
}