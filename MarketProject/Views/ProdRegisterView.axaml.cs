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

public partial class ProdRegisterView : UserControl
{
    // Cria um modelo de método tipo ProductAddedDelegate,
    // havendo o parâmetro produto, permitindo valores null
    public delegate void ProductAddedDelegate(Product? product);
    
    // Armazena todos os método criados do tipo ProductAddedDelegate,
    // Executando todas ao mesmo tempo
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

            if (MinMaxViewModel.EventsMax <= MinMaxViewModel.EventsMin ||
                MinMaxViewModel.WeekdaysMax <= MinMaxViewModel.WeekdaysMin ||
                MinMaxViewModel.WeekendsMax <= MinMaxViewModel.WeekendsMin)
            {
                throw new MaxMinException("Não é possível que o Estoque máximo seja inferior ao estoque mínimo.");
            }

            var newproduct = new Product(gtinCode, NameTextBox.Text, SupplyTextBox.Text, Prodprice,
                (UnitComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                new Range(MinMaxViewModel.WeekdaysMin, MinMaxViewModel.WeekdaysMax),
                new Range(MinMaxViewModel.WeekendsMin, MinMaxViewModel.WeekendsMax),
                new Range(MinMaxViewModel.EventsMin, MinMaxViewModel.EventsMax), DescriptionTextBox.Text, total);
            
            var msgbox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                CanResize = false,
                ShowInCenter = true,
                ContentTitle = "Novo Produto Adicionado!",
                Icon = MsBox.Avalonia.Enums.Icon.Success,
                ContentMessage = $"O produto \"{NameTextBox.Text}\" foi acrescentado ao estoque com êxito!",
                Markdown = false,
                MaxHeight = 800,
                MaxWidth = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ButtonDefinitions = ButtonEnum.Ok,
            });
            
            await msgbox.ShowAsPopupAsync(this);
        }
        catch (FormatException)
        {
             var msgboxError = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
             {
                 CanResize = false,
                 ShowInCenter = true,
                 ContentTitle = "Novo Produto Adicionado!",
                 ContentHeader = null,
                 Icon = MsBox.Avalonia.Enums.Icon.Error,
                 ContentMessage = "Erro: Verifique se os campos digitados estão corretos e tente novamente",
                 Markdown = false,
                 MaxHeight = 800,
                 MaxWidth = 500,
                 SizeToContent = SizeToContent.Manual,
                 WindowStartupLocation = WindowStartupLocation.CenterScreen,
                 CloseOnClickAway = false,
                 ButtonDefinitions = ButtonEnum.Ok,
             });
        
            await msgboxError.ShowAsPopupAsync(this);
        }
        catch (MaxMinException ex)
        {
            
            var ErrorMessageBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                WindowIcon = null,
                CanResize = false,
                ShowInCenter = true,
                ContentTitle = "Valor máximo do estoque é superior ao mínimo",
                ContentHeader = null,
                ContentMessage = ex.Message,
                Markdown = false,
                MaxWidth = 500,
                MaxHeight = 800,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Topmost = false,
                InputParams = null,
                CloseOnClickAway = false,
                Icon = MsBox.Avalonia.Enums.Icon.Warning,
                ButtonDefinitions = ButtonEnum.Ok
            });
            await ErrorMessageBox.ShowAsPopupAsync(this); 
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
        //Console.WriteLine(result[0].Path);
    }

    private void ReturnButton(object sender, RoutedEventArgs e)
    {
        //ProductAdded?.Invoke(null);
        var prodView = (Window)Parent;
        prodView?.Close();
    }

    private async void CleanTextBoxButton(object sender, RoutedEventArgs e)
    {
        var ClearMessageBox = MessageBoxManager.GetMessageBoxStandard("Limpar todos os campos de texto", "Você realmente deseja limpar todos os campos de texto?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Warning);
        
        var res = await ClearMessageBox.ShowAsPopupAsync(this);
        if (res == ButtonResult.Yes)
        {
            GtinTextBox.Text = "0";
            NameTextBox.Text = null;
            DescriptionTextBox.Text = null;
            PriceTextBox.Text = "0";
            QuantityTextBox.Text = "0";
            SupplyTextBox.Text = null;
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
    }
}