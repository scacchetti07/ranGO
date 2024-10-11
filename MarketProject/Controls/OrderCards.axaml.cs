using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media;

namespace MarketProject.Controls;

public partial class OrderCards : UserControl
{
    public OrderCards()
    {
        InitializeComponent();
    }

    public Border GenerateOrderTag(OrderStatusEnum statusEnum = OrderStatusEnum.New)
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
}

public enum OrderStatusEnum
{
    New,
    Preparing,
    Closed
}