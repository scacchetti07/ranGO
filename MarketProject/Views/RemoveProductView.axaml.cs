using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarketProject.Extensions;
using MongoDB.Driver;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using ctrl = MarketProject.Controllers.StorageController;

namespace MarketProject.Views;

public partial class RemoveProductView : Window
{
    public RemoveProductView()
    {
        InitializeComponent();
        this.ResponsiveWindow();
        RemoveTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        GtinIdTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
    }

    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }
    
    private async void RemoveProductButton(object sender, RoutedEventArgs e)
    {
        if (GtinIdTextBox.Text is null && RemoveTextBox.Text is null && ProductNameTextBox.Text is null) return;

        try
        {
            var product = ctrl.FindProductAsync(long.Parse(GtinIdTextBox.Text!));
            var remove = int.Parse(RemoveTextBox.Text!);
            if (remove > product.Total) return;

            ctrl.RemoveTotalProduct(product, remove);
            
            var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = $"Produto {product.Name} foi atualizado!",
                ContentMessage = $"O produto {product.Name} teve seu estoque atual atualizado!\nEstoque atual: {product.Total}",
                ButtonDefinitions = ButtonEnum.Ok, 
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                CanResize = false,
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });

            await msgBox.ShowAsync().ConfigureAwait(false);
        }
        catch (Exception)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Produto não encontrado",
                ContentMessage = $"O produto com código gtin \"{GtinIdTextBox}\" não foi encontrado.\nProcure outro ou digite novamente...",
                ButtonDefinitions = ButtonEnum.Ok, 
                Icon = MsBox.Avalonia.Enums.Icon.Error,
                CanResize = false,
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });

            await msgBox.ShowAsync().ConfigureAwait(false);
        }
        
    }

    private void ReturnStorage(object sender, RoutedEventArgs e) => this.Close();
}