using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.Vulkan;
using MarketProject.Controllers;
using MarketProject.Extensions;
using MarketProject.Models;
using StorageController = MarketProject.Controllers.StorageController;
using MarketProject.Models.Exceptions;
using MarketProject.ViewModels;
using MongoDB.Driver.Core.Misc;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Views;

public partial class ProdRegisterView : Window
{
    public delegate void ProductAddedDelegate(Product? product);
    public event ProductAddedDelegate? ProductAdded;
    
    // Implementando as funções do ProdRegisterViewmModel por meio do DataContext
    public RegisterMinMaxViewModel MinMaxViewModel => (MinMaxView.DataContext as RegisterMinMaxViewModel)!;
    
    public ProdRegisterView()
    {
        InitializeComponent();
        this.ResponsiveWindow();
        GtinTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        PriceTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        QuantityTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);

        SupplyAutoCompleteBox.ItemsSource = Database.SupplyList.Select(s => s.Name);
    }

    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }
    
    private async void AddProductButton(object sender, RoutedEventArgs e)
    {
        try
        {
            long gtinCode = Convert.ToInt64(GtinTextBox.Text);
            double Prodprice = Convert.ToDouble(PriceTextBox.Text.Replace("_", ""));
            int total = Convert.ToInt32(QuantityTextBox.Text);
            
            if (MinMaxViewModel.EventsMax <= MinMaxViewModel.EventsMin || MinMaxViewModel.WeekdaysMax <= MinMaxViewModel.WeekdaysMin ||
                MinMaxViewModel.WeekendsMax <= MinMaxViewModel.WeekendsMin)
                throw new MaxMinException();

            var newproduct = new Product(gtinCode, NameTextBox.Text, Prodprice,
                (UnitComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                new Range<int>(MinMaxViewModel.WeekdaysMin, MinMaxViewModel.WeekdaysMax),
                new Range<int>(MinMaxViewModel.WeekendsMin, MinMaxViewModel.WeekendsMax),
                new Range<int>(MinMaxViewModel.EventsMin, MinMaxViewModel.EventsMax), DescriptionTextBox.Text, total);
            
            var oldProductId = StorageController.FindProduct(newproduct.Gtin);
            if (oldProductId is not null)
            {
                newproduct.Id = oldProductId.Id;
                StorageController.UpdateStorage(newproduct, SupplyAutoCompleteBox.Text);
                ProductAdded?.Invoke(newproduct);
                return;
            }
            StorageController.AddProduct(newproduct,SupplyAutoCompleteBox.Text);
            ProductAdded?.Invoke(newproduct);
        }
        catch (MaxMinException)
        {
            // Adicionar Data Validation
            var errorMinMaxMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Erro: Número máximo do estoque é inferiro ao mínimo.",
                ContentMessage = "Não foi possível adicionar o produto por seu valor Máximo no estoque ser inferior ao Mínimo!",
                ButtonDefinitions = ButtonEnum.Ok, 
                Icon = MsBox.Avalonia.Enums.Icon.Error,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            await errorMinMaxMsgBox.ShowAsync().ConfigureAwait(false);
        }
        catch (Exception)
        {
            var errorMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Algum erro inesperado ocorreu!",
                ContentMessage = "Ocorreu algum erro inesperado no sistema enquanto o produo era adicionado.\nTente Novamente!",
                ButtonDefinitions = ButtonEnum.Ok, 
                Icon = MsBox.Avalonia.Enums.Icon.Error,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            await errorMsgBox.ShowAsync();
        }
    }

    private async void AddImageProduct(object sender, RoutedEventArgs e)
    {
        FilePickerOpenOptions fileoption = new()
        {
            AllowMultiple = false,
            Title = "Selecione a foto do produto",
            FileTypeFilter = new List<FilePickerFileType>
            {
                FilePickerFileTypes.ImagePng, FilePickerFileTypes.ImageJpg
            }
        };
        var result = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(fileoption);
    }

    private async void ReturnButton(object sender, RoutedEventArgs e)
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
                ContentMessage = "Ainda existem dados escritos nos campos de cadastro,\nDeixe-os todos em branco para retornar a tela inicial.",
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

    private async void CleanTextBoxButton(object sender, RoutedEventArgs e)
    {
        var ClearMessageBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Limpar campos...",
            ContentMessage = "Você realmente deseja limpar todos os campos de texto?",
            ButtonDefinitions = ButtonEnum.YesNo, 
            Icon = MsBox.Avalonia.Enums.Icon.Warning,
            CanResize = false,
            ShowInCenter = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        
        var res = await ClearMessageBox.ShowAsync();
        if (res == ButtonResult.No) return;
        ClearTextBox();
    }

    private void ClearTextBox()
    {
        GtinTextBox.Text = null;
        NameTextBox.Text = null;
        DescriptionTextBox.Text = null;
        PriceTextBox.Text = null;
        QuantityTextBox.Text = null;
        SupplyAutoCompleteBox.Text = null;
        UnitComboBox.SelectedItem = 0;

        MinMaxView.MinTextBox.Text = "0";
        MinMaxView.MaxTextBox.Text = "0";
        MinMaxViewModel.WeekdaysMin = 0;
        MinMaxViewModel.WeekdaysMax = 0;
        MinMaxViewModel.WeekendsMin = 0;
        MinMaxViewModel.WeekendsMax = 0;
        MinMaxViewModel.EventsMin = 0;
        MinMaxViewModel.EventsMax = 0;
    }
    
    private List<string> GetTextBoxes() 
        => new() {
            NameTextBox.Text,
            DescriptionTextBox.Text,
            GtinTextBox.Text,
            SupplyAutoCompleteBox.Text,
            QuantityTextBox.Text,
        };
}