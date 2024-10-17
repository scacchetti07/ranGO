using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;
using MarketProject.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OrderCtrl = MarketProject.Controllers.OrderController;

namespace MarketProject.Views;

public partial class OrderHomeView : UserControl
{
    private static readonly StyledProperty<ObservableCollection<Orders>> OrdersProperty =
        AvaloniaProperty.Register<OrderHomeView, ObservableCollection<Orders>>(nameof(OrderController));
    
    private Button? _selectedButton;
    private OrderHomeViewModel _vm => DataContext as OrderHomeViewModel;

    private ObservableCollection<Orders> _ordersList
    {
        get => GetValue(OrdersProperty);
        set => SetValue(OrdersProperty, value);
    }
    public OrderHomeView()
    {
        InitializeComponent();
        UpdateOrders();
        toggleSelectedButton(AllOrdersButton);
        OrdersProperty.Changed.AddClassHandler<OrderHomeView>((_, _) => UpdateOrders());
    }

    private void unSelectButton() => _selectedButton?.Classes.Remove("IsSelected");
    
    private void selectButton(Button? btn)
    {
        _selectedButton = btn;
        _selectedButton!.Classes.Add("IsSelected");
    }
    
    private void toggleSelectedButton(Button? btn)
    {
        unSelectButton();
        selectButton(btn);
    }
    public async void UpdateOrders()
    {
        OrderCardsPanel.Children.Clear();
        var ordersList = await OrderController.FindOrders();
        if (ordersList is null) return;
        foreach (Orders order in ordersList)
        {
            OrderCardsPanel.Children.Add(_vm.OrderToCard(order));
        }
    }
    public void UpdateOrders(IEnumerable<Orders> searchOrders)
    {
        OrderCardsPanel.Children.Clear();
        foreach (Orders order in searchOrders)
        {
            OrderCardsPanel.Children.Add(_vm.OrderToCard(order));
        }
    }

    private void AllOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        UpdateOrders();
    }

    private async void NewOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        var newStatusOrders = await OrderCtrl.FindOrders(OrderStatusEnum.New);
        UpdateOrders(newStatusOrders);
    }

    private async void PreparingOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        var preparingStatusOrders = await OrderCtrl.FindOrders(OrderStatusEnum.Preparing);
        UpdateOrders(preparingStatusOrders);
    }

    private async void ClosedOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        var preparingStatusOrders = await OrderCtrl.FindOrders(OrderStatusEnum.Closed);
        UpdateOrders(preparingStatusOrders);
    }
    private async void AddOrder_OnClick(object sender, RoutedEventArgs e)
    {
        ManageOrdersView manageOrdersView = new()
        {
            Title = "Adicionar Pedido - ranGO!"
        };
        manageOrdersView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!);
        var newOrder = await manageOrdersView.GetOrder();
        OrderCtrl.AddNewOrder(newOrder);
        UpdateOrders();
    }

    private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = SearchTextBox.Text;
        if (keyword.Length < 1)
        {
            UpdateOrders();
            return;
        }
        
        var tableNumber = int.TryParse(keyword, out int tbl);
        IEnumerable<Orders> searchedList;
        if (tableNumber)
            searchedList = OrderCtrl.OrdersList.Where(o => o.TableNumber.ToString().Contains($"{tbl}"));
        else
            searchedList = OrderCtrl.OrdersList.Where(o => o.Id.ToLower().EndsWith(keyword.ToLower()));   
           
        
        UpdateOrders(searchedList);
    }
}