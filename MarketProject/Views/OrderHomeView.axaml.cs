using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MarketProject.Models;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class OrderHomeView : UserControl
{
    private OrderHomeViewModel _vm => DataContext as OrderHomeViewModel;
    public OrderHomeView()
    {
        InitializeComponent();
    }

    public void UpdateOrders()
    {
        OrderCardsPanel.Children.Clear();
        
    }
}