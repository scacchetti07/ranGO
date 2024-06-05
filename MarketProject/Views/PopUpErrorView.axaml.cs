using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class PopUpErrorView : Window
{
    
    public PopUpErrorView(string msg)
    {
        InitializeComponent();
        lblmsg.Content = msg;
    }


    private void btnExit(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}