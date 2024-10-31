using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.Vulkan;
using MarketProject.Controllers;
using MarketProject.Extensions;
using MarketProject.Models;
using StorageController = MarketProject.Controllers.StorageController;
using MarketProject.ViewModels;
using MongoDB.Driver.Core.Misc;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Timer = System.Timers.Timer;

namespace MarketProject.Views;

public partial class ProductAddView : Window
{
    public delegate void ProductAddedDelegate(Product? product);
    public event ProductAddedDelegate? ProductAdded;
    public RegisterMinMaxViewModel MinMaxViewModel => (MinMaxView.DataContext as RegisterMinMaxViewModel)!;
    
    public ProductAddView(Product selectedProducts = null)
    {
        InitializeComponent();
        this.ResponsiveWindow();
        GtinTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        PriceTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        QuantityTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        if (selectedProducts is not null)
        {
            AddButton.Content = "Editar";
            SupplyContent.IsVisible = false;
            
            var supplyName = SupplyController.GetSupplyNameByProduct(selectedProducts);
        
            GtinTextBox.Text = selectedProducts.Gtin.ToString();
            GtinTextBox.IsEnabled = false;
        
            NameTextBox.Text = selectedProducts.Name;
            DescriptionTextBox.Text = selectedProducts.Description;
            ValidityDatePicker.SelectedDate = selectedProducts.Validity;
        
            PriceTextBox.Text = selectedProducts.Price.ToString("f2").PadLeft(6, '_');
            var item = UnitComboBox.Items.SingleOrDefault(u => (u as ComboBoxItem).Content.ToString() == selectedProducts.Unit);
            UnitComboBox.SelectedIndex = UnitComboBox.Items.IndexOf(item);
        
            SupplyAutoCompleteBox.Text = supplyName;
            QuantityTextBox.Text = selectedProducts.Total.ToString();
        
            MinMaxView.MinTextBox.Text = selectedProducts.Weekdays.Min.ToString();
            MinMaxViewModel.WeekdaysMin = selectedProducts.Weekdays.Min;
            MinMaxView.MaxTextBox.Text = selectedProducts.Weekdays.Max.ToString();
            MinMaxViewModel.WeekdaysMax = selectedProducts.Weekdays.Max;
            MinMaxViewModel.EventsMin = selectedProducts.Events.Min;
            MinMaxViewModel.EventsMax = selectedProducts.Events.Max;
            MinMaxViewModel.WeekendsMin = selectedProducts.Weekends.Min;
            MinMaxViewModel.WeekendsMax = selectedProducts.Weekends.Max;

            return;
        }
        
        SupplyAutoCompleteBox.AddHandler(KeyDownEvent, (sender, e) =>
        {
            if (sender is not AutoCompleteBox autoComplete)
                return;
            if (e.Key != Key.Tab || autoComplete.Text is null || autoComplete.Text.Trim() == "")
                return;

            string item = autoComplete.ItemsSource!
                .Cast<string>()
                .FirstOrDefault(item =>
                    autoComplete.TextFilter?.Invoke(autoComplete.Text, item) ?? true);
            if (item is null)
                return;

            autoComplete.Text = item;
            autoComplete.CaretIndex = autoComplete.Text.Length;
            e.Handled = true;
        }, RoutingStrategies.Tunnel);

        SupplyAutoCompleteBox.ItemsSource = Database.SupplyList.Select(s => s.Name);
        
        SupplyAutoCompleteBox.Loaded += (_, _) =>
        {
            TextBox textBox = SupplyAutoCompleteBox.GetTemplateChildren().Single(control => control is TextBox) as TextBox;
            textBox!.Bind(TextBox.TextProperty, new Binding()
            {
                Path = "SupplyName",
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        };      
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
                throw new Exception("Estoque máximo não pode ser inferior ao mínimo.");

            if (SupplyController.FindSupplyByName(SupplyAutoCompleteBox.Text) is null)
                throw new Exception("O Fornecedor digitado não existe no sistema!");

            if (ValidityDatePicker.SelectedDate.Value < DateTimeOffset.Now)
                throw new Exception($"A data inserida é inferior a data atual '{DateTime.Now}'!");
            
            var newproduct = new Product(gtinCode, NameTextBox.Text, Prodprice,
                (UnitComboBox.SelectedItem as ComboBoxItem).Content.ToString(), ValidityDatePicker.SelectedDate.Value.DateTime.Date,
                new Range<int>(MinMaxViewModel.WeekdaysMin, MinMaxViewModel.WeekdaysMax),
                new Range<int>(MinMaxViewModel.WeekendsMin, MinMaxViewModel.WeekendsMax),
                new Range<int>(MinMaxViewModel.EventsMin, MinMaxViewModel.EventsMax), DescriptionTextBox.Text, total);
            
            var oldProductId = StorageController.FindProduct(newproduct.Gtin);
            if (oldProductId is not null && AddButton.Content == "Editar")
            {
                newproduct.Id = oldProductId.Id;
                StorageController.UpdateStorage(newproduct, SupplyAutoCompleteBox.Text);
                ProductAdded?.Invoke(newproduct);
                return;
            }
            if (oldProductId is not null)
                throw new Exception("Código GTIN digitado já existe no sistema!");
            StorageController.AddProduct(newproduct,SupplyAutoCompleteBox.Text);
            ProductAdded?.Invoke(newproduct);
        }
        catch (Exception ex)
        {
            var errorMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Erro ao cadastrar o produto no estoque!",
                ContentMessage = ex.Message,
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
        ValidityDatePicker.SelectedDate = null;
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
    
    private void ValidityDatePicker_OnSelectedDateChanged(object sender, DatePickerSelectedValueChangedEventArgs e)
    {
        var date = ValidityDatePicker.SelectedDate.Value.DateTime;
        var today = DateTime.Now;
        
        if (date < today)
            ProductAdded?.Invoke(null);
    }
}