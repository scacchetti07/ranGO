using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using DynamicData;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;
using MarketProject.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using OrderCtrl = MarketProject.Controllers.OrderController;

namespace MarketProject.Views;

public partial class OrderHomeView : UserControl
{
    private static readonly StyledProperty<ObservableCollection<Orders>> OrdersProperty =
        AvaloniaProperty.Register<OrderHomeView, ObservableCollection<Orders>>(nameof(OrderController));

    private Button? _selectedButton;
    private ICommand _returnCommand;
    private OrderHomeViewModel _vm => DataContext as OrderHomeViewModel;

    public delegate void OrderSelectedChanged(OrderStatusEnum? order);

    public event OrderSelectedChanged? OrderSelected;

    public OrderHomeView()
    {
        InitializeComponent();
        toggleSelectedButton(AllOrdersButton);
        UpdateOrders();
        OrdersProperty.Changed.AddClassHandler<OrderHomeView>((_, _) => UpdateOrders());

        OrderSelected += (order) =>
        {
            if (order is null)
                UpdateOrders();
            else
                UpdateOrders(order);

            if (order == OrderStatusEnum.Closed) {
                DeleteOrderButton.IsVisible = true;
                OrderHomeButtons.Margin = new Thickness(243, 0); 
            }
            else {
                DeleteOrderButton.IsVisible = false;
                OrderHomeButtons.Margin = new Thickness(314, 0);  
            }
        };
    }

    private void unSelectButton()
    {
        _selectedButton?.Classes.Remove("IsSelected");
        OrderCardsPanel.Classes.Remove("AnimationOrderTransition");
    }

    private void selectButton(Button? btn)
    {
        _selectedButton = btn;
        _selectedButton!.Classes.Add("IsSelected");
        OrderCardsPanel.Classes.Add("AnimationOrderTransition");
    }

    private void toggleSelectedButton(Button? btn)
    {
        unSelectButton();
        selectButton(btn);
    }

    private async void UpdateOrders()
    {
        OrderCardsPanel.Children.Clear();
        var ordersList = await OrderController.FindOrders();
        if (ordersList is null) return;
        foreach (Orders order in ordersList)
            OrderCardsPanel.Children.Add(_vm.OrderToCard(order));
    }

    private async void UpdateOrders(OrderStatusEnum? orderStatus)
    {
        if (orderStatus is null) return;

        var searchOrders = await OrderCtrl.FindOrders((OrderStatusEnum)orderStatus);
        OrderCardsPanel.Children.Clear();
        foreach (Orders order in searchOrders)
            OrderCardsPanel.Children.Add(_vm.OrderToCard(order));
    }

    private void UpdateOrders(IEnumerable<Orders> searchedList)
    {
        OrderCardsPanel.Children.Clear();
        foreach (Orders order in searchedList)
            OrderCardsPanel.Children.Add(_vm.OrderToCard(order));
    }

    private void AllOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        OrderSelected?.Invoke(null);
    }

    private void NewOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        OrderSelected?.Invoke(OrderStatusEnum.New);
    }

    private void PreparingOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        OrderSelected?.Invoke(OrderStatusEnum.Preparing);
    }

    private void ClosedOrders_OnClick(object sender, RoutedEventArgs e)
    {
        toggleSelectedButton(sender as Button);
        OrderSelected?.Invoke(OrderStatusEnum.Closed);
    }

    private async void AddOrder_OnClick(object sender, RoutedEventArgs e)
    {
        ManageOrdersView manageOrdersView = new() { Title = "Adicionar Pedido - ranGO!" };
        manageOrdersView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!);
        var newOrder = await manageOrdersView.GetOrder();
        manageOrdersView.OrderAdded += (order) =>
        {
            var lastId = new string(order.Id.TakeLast(4).ToArray());
            AddPopup.IsOpen = true;
            AddProdLabel.Content = "Novo Pedido Adicionado!";
            ContentAddTextBlock.Text = $"Pedido de Id '#{lastId}' foi adicionado!";
        };
        OrderCtrl.AddNewOrder(newOrder);
        if (_selectedButton.Name == "AllOrdersButton")
            OrderSelected?.Invoke(null);
        
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

    private void FoodMenuButton_OnClick(object sender, RoutedEventArgs e)
    {
        FoodMenuView foodMenuView = new() { HorizontalAlignment = HorizontalAlignment.Right };
        
        FoodMenuViewPanel.Children.Add(foodMenuView);
        FoodMenuViewPanel.Classes.Add("FoodMenuAnimation");
        
        foodMenuView.ExitButton.Command = ReactiveCommand.Create(() =>
        {
            FoodMenuViewPanel.Classes.Clear();
            FoodMenuViewPanel.Children.Clear();
        });
    }

    private async void DeleteOrderButton_OnClick(object sender, RoutedEventArgs e)
    {
        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Você tem certeza disso?",
            ContentMessage = "Tem certeza que realmente quer excluir TODOS os pedidos concluídos do sistema?",
            ButtonDefinitions = ButtonEnum.YesNo,
            Icon = Icon.Error,
            CanResize = false,
            ShowInCenter = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        var result = await msgBox.ShowAsync().ConfigureAwait(false);
        if (result == ButtonResult.No) return;
        
        Dispatcher.UIThread.Post(async () =>
        {
            var searchOrders = await OrderCtrl.FindOrders(OrderStatusEnum.Closed);
            foreach (var order in searchOrders)
                OrderCtrl.DeleteOrder(order);
            UpdateOrders(OrderStatusEnum.Closed);
        }, DispatcherPriority.Background);
        
    }
}