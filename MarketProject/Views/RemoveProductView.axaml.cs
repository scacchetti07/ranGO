using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MarketProject.Extensions;
using MarketProject.Models;
using MongoDB.Driver;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using StorageCtrl = MarketProject.Controllers.StorageController;

namespace MarketProject.Views;

public partial class RemoveProductView : Window
{
    public delegate void ProductDeletedDelegate(Product? product);
    public event ProductDeletedDelegate? ProductDeleted;
    public RemoveProductView()
    {
        InitializeComponent();
        RemoveTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        GtinIdTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);

        ProductNameTextBox.ItemsSource = Database.ProductsList.Select(p => p.Name);
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
            var product = StorageCtrl.FindProduct(long.Parse(GtinIdTextBox.Text!));
            var remove = int.Parse(RemoveTextBox.Text!);
            if (remove > product.Total) return;

            StorageCtrl.RemoveTotalProduct(product, remove);
            ProductDeleted?.Invoke(product);
            ClearTextBox();
            
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

    private void ReturnStorage(object sender, RoutedEventArgs e)
    {
        List<string> textBoxes = GetTextBoxes();
        if (textBoxes.TrueForAll(txt => string.IsNullOrEmpty(txt)))
        {
            Close();
            return;
        }
        
        Dispatcher.UIThread.Post(async () =>
        {
            var checkMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Dados ainda digitados.",
                ContentMessage = "Ainda existem dados escritos nos campos \nRealmente deseja sair desta tela?",
                ButtonDefinitions = ButtonEnum.YesNo, 
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            var result = await checkMsgBox.ShowAsync();
            if (result == ButtonResult.Yes) Close();
            
        }, DispatcherPriority.Background);     
    }
    private List<string> GetTextBoxes() 
        => new() {
            ProductNameTextBox.Text,
            RemoveTextBox.Text,
            GtinIdTextBox.Text,
        };
    
    private void ProductNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = ProductNameTextBox.Text;
        var prod = StorageCtrl.FindProductByNameAsync(keyword);
        if (prod is null)
        {
            GtinIdTextBox.Text = "";
            return;
        }
        GtinIdTextBox.Text = prod.Gtin.ToString();
    }

    private void ClearTextBox()
    {
        ProductNameTextBox.Text = "";
        GtinIdTextBox.Text = "";
        RemoveTextBox.Text = "";
    }
}