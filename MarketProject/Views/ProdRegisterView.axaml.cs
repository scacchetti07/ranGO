using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DialogHostAvalonia;
using MarketProject.Extensions;
using MarketProject.Models;
using MarketProject.Views;
using MarketProject.Models.Exceptions;
using MarketProject.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Views;

public partial class ProdRegisterView : Window
{
    public delegate void ProductAddedDelegate(Product? product);
    public event ProductAddedDelegate? ProductAdded;
    
    // Implementando as funções do ProdRegisterViewmModel por meio do DataContext
    public ProdRegisterViewModel ViewModel => (DataContext as ProdRegisterViewModel)!;
    public RegisterMinMaxViewModel MinMaxViewModel => (MinMaxView.DataContext as RegisterMinMaxViewModel)!;

    public StorageViewModel StorageViewModel => (DataContext as StorageViewModel)!;
    
    public ProdRegisterView()
    {
        InitializeComponent();
        
        GtinTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        
        PriceTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        
        QuantityTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
            
    }

    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }
    
    private void btnAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        // É instanciando um objeto do tipo Product,
        // Recebendo todos os campos digitados na View de resgistro,
        //Product product = new Product(txtName.Text,int.Parse(txtQtd.Text),txtSup.Text,((cbxStatus.SelectedItem as ComboBoxItem).Content.ToString()),int.Parse(txtMin.Text),int.Parse(txtMax.Text));
        // Caso exista funções em ProductAddedDelegate, ele irá enviar os produtos registrados,
        // Também retornando a tela de estoque.
        //ProductAdded?.Invoke(product);
    }

    private async void AddProductButton(object sender, RoutedEventArgs e)
    {
        try
        {
            var gtinCode = Convert.ToInt32(GtinTextBox.Text);
            var Prodprice = Convert.ToDecimal(PriceTextBox.Text);
            var total = Convert.ToInt32(QuantityTextBox.Text);

            if (MinMaxViewModel.EventsMax <= MinMaxViewModel.EventsMin || MinMaxViewModel.WeekdaysMax <= MinMaxViewModel.WeekdaysMin ||
                MinMaxViewModel.WeekendsMax <= MinMaxViewModel.WeekendsMin)
                throw new MaxMinException();

            var checkProductsMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams()
            {
                ContentHeader = "Adicionando o Produto",
                ContentMessage = $"Quer mesmo adicionar \"{NameTextBox.Text}\" ao estoque?",
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                ButtonDefinitions = ButtonEnum.YesNo,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            });
            var checkResult = await checkProductsMsgBox.ShowAsync();

            if (checkResult == ButtonResult.No) return;
            
            var newproduct = new Product(gtinCode, NameTextBox.Text, new Supply(), Prodprice,
                (UnitComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                new Range(MinMaxViewModel.WeekdaysMin, MinMaxViewModel.WeekdaysMax),
                new Range(MinMaxViewModel.WeekendsMin, MinMaxViewModel.WeekendsMax),
                new Range(MinMaxViewModel.EventsMin, MinMaxViewModel.EventsMax), DescriptionTextBox.Text, total);

            ProductAdded?.Invoke(newproduct);
            
            // Alterar msgBox por uma notificação na cor verde indicando que o produto foi adicionado ao estoque.
            var msgbox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Novo Produto Adicionado!",
                ContentMessage = $"O produto \"{NameTextBox.Text}\" foi adicionado ao estoque!",
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = MsBox.Avalonia.Enums.Icon.Success,
                Markdown = false,
                CanResize = false,
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            });
            await msgbox.ShowAsync();
        }
        catch (MaxMinException)
        {
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
            });
            await errorMinMaxMsgBox.ShowAsync();
        }
        catch (Exception ex)
        {
            var errorMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Erro inesperado ocorreu!",
                ContentMessage = "Ocorreu algum erro inesperado no sistema enquanto o produo era adicionado.\nTente Novamente!",
                ButtonDefinitions = ButtonEnum.Ok, 
                Icon = MsBox.Avalonia.Enums.Icon.Error,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            });
            Console.WriteLine(ex.Message);
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

    private void ReturnButton(object sender, RoutedEventArgs e)
    {
        this.Close();
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
        });
        
        var res = await ClearMessageBox.ShowAsync();
        if (res == ButtonResult.No) return;
        
        GtinTextBox.Text = null;
        NameTextBox.Text = null;
        DescriptionTextBox.Text = null;
        PriceTextBox.Text = null;
        QuantityTextBox.Text = null;
        SupplyTextBox.Text = null;
        UnitComboBox.SelectedItem = 0;

        MinMaxView.MinTextBox.Text = null;
        MinMaxView.MaxTextBox.Text = null;
        MinMaxViewModel.WeekdaysMin = 0;
        MinMaxViewModel.WeekdaysMax = 0;
        MinMaxViewModel.WeekendsMin = 0;
        MinMaxViewModel.WeekendsMax = 0;
        MinMaxViewModel.EventsMin = 0;
        MinMaxViewModel.EventsMax = 0;
    }
}