using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using MarketProject.Models;
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
    
    public ProdRegisterView()
    {
        InitializeComponent();
    }

    // Botão de Retornar a tela do estoque
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        // Verifica se existe algum método no ProductAddedDelegate,
        // Caso exista, ele irá enviar null, retornando a última tela.
        ProductAdded?.Invoke(null);
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

            var newproduct = new Product(gtinCode, NameTextBox.Text, SupplyTextBox.Text, Prodprice,
                (UnitComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                new Range(MinMaxViewModel.WeekdaysMin, MinMaxViewModel.WeekdaysMax),
                new Range(MinMaxViewModel.WeekendsMin, MinMaxViewModel.WeekendsMax),
                new Range(MinMaxViewModel.EventsMin, MinMaxViewModel.EventsMax));

            var msgbox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                CanResize = false,
                ShowInCenter = true,
                ContentTitle = "Novo Produto Adicionado!",
                ContentHeader = null,
                Icon = Icon.Success,
                ContentMessage = $"O produto \"{NameTextBox.Text}\" foi acrescentado ao estoque com êxito!",
                Markdown = false,
                MaxHeight = 800,
                MaxWidth = 500,
                SystemDecorations = SystemDecorations.Full,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                CloseOnClickAway = false,
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
              Icon = Icon.Error,
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
        Console.WriteLine(result[0].Path);
    }   
}