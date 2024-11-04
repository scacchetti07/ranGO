using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Controls;

public partial class OrderCards : UserControl
{
    public static readonly StyledProperty<string> WaiterNameProperty =
        AvaloniaProperty.Register<OrderCards, string>(nameof(WaiterName));

    public static readonly StyledProperty<string> FoodOrderNamesProperty =
        AvaloniaProperty.Register<OrderCards, string>(nameof(FoodOrderNames));

    public static readonly StyledProperty<int> TableNumberProperty =
        AvaloniaProperty.Register<OrderCards, int>(nameof(TableNumber));

    public static readonly StyledProperty<string> OrderIdProperty =
        AvaloniaProperty.Register<OrderCards, string>(nameof(Id));

    public static readonly StyledProperty<OrderStatusEnum> OrderStatusProperty =
        AvaloniaProperty.Register<OrderCards, OrderStatusEnum>(nameof(OrderStatus));

    public string WaiterName
    {
        get => GetValue(WaiterNameProperty);
        set => SetValue(WaiterNameProperty, value);
    }

    public string FoodOrderNames
    {
        get => GetValue(FoodOrderNamesProperty);
        set => SetValue(FoodOrderNamesProperty, value);
    }

    public int TableNumber
    {
        get => GetValue(TableNumberProperty);
        set => SetValue(TableNumberProperty, value);
    }

    public string Id
    {
        get => GetValue(OrderIdProperty);
        set => SetValue(OrderIdProperty, value);
    }

    public OrderStatusEnum OrderStatus
    {
        get => GetValue(OrderStatusProperty);
        set => SetValue(OrderStatusProperty, value);
    }

    public OrderCards()
    {
        InitializeComponent();
        WaiterNameProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        FoodOrderNamesProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        TableNumberProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        OrderIdProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        OrderStatusProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());

        Application.Current.ActualThemeVariantChanged += (_, _) => { UpdateBackgroundColorTheme(); };
    }

    private void UpdateOrderCard()
    {
        WaiterNameLabel.Content = WaiterName;
        OrderFoodsLabel.Content = FoodOrderNames;
        TableNumberLabel.Content = TableNumber.ToString("D2").Insert(0, "Mesa ");
        OrderIdLabel.Content = Id != null ? string.Join("", Id.TakeLast(4)).Insert(0, "#") : "";
        OrderStatusStackPanel.Children.Clear();
        OrderStatusStackPanel.Children.Add(GenerateOrderTag(OrderStatus));
        UpdateBackgroundColorTheme();

        switch (OrderStatus)
        {
            case OrderStatusEnum.New:
                OrderStatusButton.Background = Brush.Parse("#D87249");
                ButtonIconSvg.Path = "/Assets/Icons/SVG/IconCooking.svg";
                break;
            case OrderStatusEnum.Preparing:
                OrderStatusButton.Background = Brush.Parse("#6EA759");
                ButtonIconSvg.Path = "/Assets/Icons/SVG/IconCheck.svg";
                break;
            case OrderStatusEnum.Closed:
                OrderStatusButton.IsVisible = false;
                break;
        }
    }

    public void UpdateBackgroundColorTheme()
    {
        var theme = Application.Current.RequestedThemeVariant.ToString();
        switch (theme)
        {
            case "Light":
                OrderCard.Background = Brush.Parse("#BCC4D0");
                break;
            case "Dark":
                OrderCard.Background = Brush.Parse("#E8EEF7");
                break;
        }
    }

    private static Border GenerateOrderTag(OrderStatusEnum statusEnum)
    {
        TextBlock textBlock;
        //Border border;
        switch (statusEnum)
        {
            case OrderStatusEnum.New:
                textBlock = new TextBlock()
                    { Text = "Novo", Foreground = Brush.Parse("#351C12"), };
                return new Border()
                {
                    Child = textBlock,
                    Background = Brush.Parse("#59D87249"),
                    BorderBrush = Brush.Parse("#D87249"),
                    Classes = { "OrderStatusTag" }
                };
            case OrderStatusEnum.Preparing:
                textBlock = new TextBlock()
                    { Text = "Preparando", Foreground = Brush.Parse("#3C3119"), };
                return new Border()
                {
                    Child = textBlock,
                    Background = Brush.Parse("#59DCB861"),
                    BorderBrush = Brush.Parse("#DCB861"),
                    Classes = { "OrderStatusTag" }
                };
            case OrderStatusEnum.Closed:
                textBlock = new TextBlock()
                    { Text = "Fechado", Foreground = Brush.Parse("#203817"), };
                return new Border
                {
                    Child = textBlock,
                    Background = Brush.Parse("#596EA759"),
                    BorderBrush = Brush.Parse("#6EA759"),
                    Classes = { "OrderStatusTag" }
                };
        }

        return null;
    }

    private async void EditOrder_OnClick(object sender, RoutedEventArgs e)
    {
        ManageOrdersView manageOrdersView = new(Id)
        {
            Title = "Editar Pedido - ranGO!"
        };
        await manageOrdersView
            .ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!)
            .ConfigureAwait(false);
    }

    private void OrderStatus_OnClick(object sender, RoutedEventArgs e)
    {
        var orderStatus = OrderStatusEnum.Preparing;
        switch (OrderStatus)
        {
            case OrderStatusEnum.New:
                orderStatus = OrderStatusEnum.Preparing;
                OrderStatusButton.Background = Brush.Parse("#6EA759");
                ButtonIconSvg.Path = "/Assets/Icons/SVG/IconCheck.svg";
                break;
            case OrderStatusEnum.Preparing:
                orderStatus = OrderStatusEnum.Closed;
                OrderStatusButton.IsVisible = false;
                break;
        }

        Dispatcher.UIThread.Post(() =>
        {
            var actualOrder = OrderController.OrdersList.FirstOrDefault(o => o.Id.Contains(Id[1..]));
            if (actualOrder is null) return;

            OrderStatus = actualOrder.OrderStatus = orderStatus;
            OrderController.EditOrder(actualOrder);
        }, DispatcherPriority.Background);
    }
}

public enum OrderStatusEnum
{
    New,
    Preparing,
    Closed
}