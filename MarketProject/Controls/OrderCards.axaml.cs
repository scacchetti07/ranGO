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
        
    }

    private void UpdateOrderCard()
    {
        WaiterNameLabel.Content = WaiterName;
        OrderFoodsLabel.Content = FoodOrderNames;
        TableNumberLabel.Content = TableNumber.ToString("D2").Insert(0, "Mesa ");
        OrderIdLabel.Content = Id;
        OrderStatusStackPanel.Children.Clear();
        OrderStatusStackPanel.Children.Add(GenerateOrderTag(OrderStatus));
    }

    private static Border GenerateOrderTag(OrderStatusEnum statusEnum)
    {
        TextBlock textBlock;
        Border border = new();
        switch (statusEnum)
        {
            case OrderStatusEnum.New:
                textBlock = new TextBlock()
                { Text = "Novo", Foreground = Brush.Parse("#351C12"), };
                border = new Border()
                {
                    Child = textBlock,
                    Background = Brush.Parse("#59D87249"),
                    BorderBrush = Brush.Parse("#D87249"),
                    Classes = { "OrderStatusTag" }
                };
                break;
            case OrderStatusEnum.Preparing:
                textBlock = new TextBlock()
                    { Text = "Preparando", Foreground = Brush.Parse("#3C3119"), };
                border = new Border()
                {
                    Child = textBlock,
                    Background = Brush.Parse("#59DCB861"),
                    BorderBrush = Brush.Parse("#DCB861"),
                    Classes = { "OrderStatusTag" }
                };
                break;
            default:
                return null;
            // case OrderStatusEnum.Closed:
            //     textBlock = new TextBlock()
            //         { Text = "Fechado", Foreground = Brush.Parse("#203817"), };
            //     return new Border
            //     {
            //         Child = textBlock,
            //         Background = Brush.Parse("#596EA759"),
            //         BorderBrush = Brush.Parse("#6EA759"),
            //         Classes = { "OrderStatusTag" }
            //     };
        }
        return border;
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

    private async void CheckOrder_OnClick(object sender, RoutedEventArgs e)
    {
        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Concluir Pedido?",
            ContentMessage = "Concluir o pedido, irá resultar na remoção do mesmo, você tem certeza disso?",
            ButtonDefinitions = ButtonEnum.YesNo,
            Icon = Icon.Warning,
            CanResize = false,
            ShowInCenter = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        var result = await msgBox.ShowAsync().ConfigureAwait(false);
        if (result == ButtonResult.No) return;
        
        Dispatcher.UIThread.Post(() =>
        {
            var actualOrder = OrderController.OrdersList.FirstOrDefault(o => o.Id.Contains(Id[1..]));
            if (actualOrder is null) return;
            
            OrderController.DeleteOrder(actualOrder);
            // Atualizar a tela de que o pedido foi removido.
        },DispatcherPriority.Background);
       
    }

    private void PreparingOrder_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.OrderStatus == OrderStatusEnum.Preparing) return;
        Dispatcher.UIThread.Post(() =>
        {
            var actualOrder = OrderController.OrdersList.FirstOrDefault(o => o.Id.Contains(Id[1..]));
            if (actualOrder is null) return;

            OrderStatus = actualOrder.OrderStatus = OrderStatusEnum.Preparing;
            OrderController.EditOrder(actualOrder);
        },DispatcherPriority.Background);
       
    }
}

public enum OrderStatusEnum
{
    New,
    Preparing,
    Closed
}