using System;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MongoDB.Driver;
using ctrl = MarketProject.Controllers.StorageController;

namespace MarketProject.Views;

public partial class RemoveProductView : Window
{
    public RemoveProductView()
    {
        InitializeComponent();
        
        RemoveTotalTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        GtinIdTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
    }

    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }
    
    private async void RemoveProduct(object sender, RoutedEventArgs e)
    {
        if (GtinIdTextBox.Text is null && RemoveTotalTextBox.Text is null && ProductNameTextBox.Text is null) Close();
        
        var product = await ctrl.FindProduct(int.Parse(GtinIdTextBox.Text!));
        if (product.Count == 0 ) Close();
        foreach (var p in product)
        {
            int total = int.Parse(RemoveTotalTextBox.Text!);
            if (total > p.Total) break;
            
            ctrl.RemoveTotalProduct(p, total);
        }

    }

    private void ReturnStorage(object sender, RoutedEventArgs e) => this.Close();
}