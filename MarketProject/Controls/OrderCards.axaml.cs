using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media;
using DynamicData;
using MarketProject.Models;
using MarketProject.Views;

namespace MarketProject.Controls;

public partial class OrderCards : UserControl
{
    public static readonly StyledProperty<string> WaiterNameProperty =
        AvaloniaProperty.Register<OrderCards, string>(nameof(WaiterName));
    public static readonly StyledProperty<IEnumerable<string>> FoodOrderNamesProperty =
        AvaloniaProperty.Register<OrderCards, IEnumerable<string>>(nameof(FoodOrderNames));
    public static readonly StyledProperty<string> TableNumberProperty =
        AvaloniaProperty.Register<OrderCards, string>(nameof(TableNumber));
    public static readonly StyledProperty<string> OrderIdProperty =
        AvaloniaProperty.Register<OrderCards, string>(nameof(Id));
    public static readonly StyledProperty<Border> OrderStatusProperty =
        AvaloniaProperty.RegisterAttached<OrderCards, Border>(nameof(OrderStatusBorder), typeof(OrderStatusEnum), GenerateOrderTag(OrderStatusEnum.New));
    public string WaiterName
    {
        get => GetValue(WaiterNameProperty);
        set => SetValue(WaiterNameProperty, value);
    }
    public IEnumerable<string> FoodOrderNames
    {
        get => GetValue(FoodOrderNamesProperty);
        set => SetValue(FoodOrderNamesProperty, value);
    }
    public string TableNumber
    {
        get => GetValue(TableNumberProperty);
        set => SetValue(TableNumberProperty, value);
    }
    public string Id
    {
        get => GetValue(OrderIdProperty);
        set => SetValue(OrderIdProperty, value);
    }
    public Border OrderStatusBorder
    {
        get => GetValue(OrderStatusProperty);
        set => SetValue(OrderStatusProperty, value);
    }
    public OrderStatusEnum OrderStatus { get; set; }
    public OrderCards()
    {
        InitializeComponent();
        WaiterNameProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        FoodOrderNamesProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        TableNumberProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        OrderIdProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
        OrderStatusProperty.Changed.AddClassHandler<OrderCards>((_, _) => UpdateOrderCard());
    }

    private void UpdateOrderCard()
    {
        WaiterNameLabel.Content = WaiterName;
        OrderFoodDescriptionLabel.Content = FoodOrderNames.Take(2);
        TableNumberLabel.Content = TableNumberProperty;
        OrderIdLabel.Content = Id[..4];
        OrderStatusStackPanel.Children.Add(OrderStatusBorder);
        OrderCardContentGrid.Classes.Clear();
    }

    private static Border GenerateOrderTag(OrderStatusEnum statusEnum)
    {
        TextBlock textBlock;
        
        switch (statusEnum)
        {
            case OrderStatusEnum.New:
                textBlock = new TextBlock()
                { Text = "Novo", Foreground = Brush.Parse("#351C12"), };
                return new Border
                {
                    Child = textBlock,
                    Background = Brush.Parse("#59D87249"),
                    BorderBrush = Brush.Parse("#D87249"),
                    Classes = { "OrderStatusTag" }
                };
            case OrderStatusEnum.Preparing:
                textBlock = new TextBlock()
                    { Text = "Preparando", Foreground = Brush.Parse("#3C3119"), };
                return new Border
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
        ManageOrdersView manageOrdersView = new()
        {
            Title = "Editar Pedido - ranGO!"
        };
        // Enviar os dados do pedido para a tela de editar
        
        await manageOrdersView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!);
    }

    private void DeleteOrder_OnClick(object sender, RoutedEventArgs e)
    {
        // Remover o card somente depois que os pedidos forem adicionados e editados.
    }

    private void CheckOrder_OnClick(object sender, RoutedEventArgs e)
    {
        OrderStatusStackPanel.Children.Clear();
        OrderStatusStackPanel.Children.Add(GenerateOrderTag(OrderStatusEnum.Closed));
    }
}

public enum OrderStatusEnum
{
    New,
    Preparing,
    Closed
}