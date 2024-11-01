using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MarketProject.Models;

namespace MarketProject.Controls;

public partial class OrderDashboardCard : UserControl
{
    public OrderDashboardCard()
    {
        InitializeComponent();
        UpdateDashboardCard();
        Database.OrdersList.CollectionChanged += (_, _) => { UpdateDashboardCard(); };
    }

    private void UpdateDashboardCard()
    {
        Dispatcher.UIThread.Post(() =>
        {
            int preparingCount = Database.OrdersList.Where(o => o.OrderStatus == OrderStatusEnum.Preparing).Count();
            int totalOrders = Database.OrdersList.Count;
            ProductsProgressBar.Maximum = totalOrders;
            
            double percentegeValue = preparingCount * 100 / ProductsProgressBar.Maximum;
            DashboardCardMainContent.Text = $"{preparingCount} abertos";
            ProductsProgressBar.Value = preparingCount;
            ProgressBarPercentage.Content = $"{Math.Round(percentegeValue, 0)}%";
            
        }, DispatcherPriority.Background);
    }
    
}