using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ExCSS;
using MarketProject.Controllers;
using MarketProject.Extensions;
using MarketProject.Models;
using MarketProject.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using SupplyCtrl = MarketProject.Controllers.SupplyController;

namespace MarketProject.Views;

public partial class SupplyAddView : Window
{ 
    public List<Product> AutoCompleteSelectedProducts { get; } = [];
    
    public SupplyAddView()
    {
        InitializeComponent();
        this.ResponsiveWindow();
        
        CepMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        CnpjMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        DateLimitTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        PhoneMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);

        ProductsAutoCompleteBox.ItemsSource = Database.ProductsList.Select(p => p.Name);
    }
    
    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }

    private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
    {
        List<string> textBoxes = GetTextBoxes();
        if (textBoxes.TrueForAll(txt => string.IsNullOrEmpty(txt)) && AutoCompleteSelectedProducts.Count == 0)
        {
            Close();
            return;
        }
        
        Dispatcher.UIThread.Post(async () =>
        {
            var checkMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Dados ainda digitados.",
                ContentMessage = "Ainda existem dados escritos nos campos de cadastro,\nQuer realmente sair do cadastro?",
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

    private async void AddSupplyButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Verificar se cnpj digitado é real após implementar a API do gov.br
        // Aplicar "Data Validation" Nos campos de CNPJ e CEP caso estejam incorretos.

        if (DateLimitTextBox.Text == null) return;
        
        int dateLimit = Convert.ToInt32(DateLimitTextBox.Text);
        List<string> textBoxes = GetTextBoxes();
        
        if (textBoxes.Any(txt => string.IsNullOrEmpty(txt))) return;

        if (!AutoCompleteSelectedProducts.Any()) return;
        
        if (int.Parse(DateLimitTextBox.Text) <= 0)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Prazo inferior ou igual a 0",
                ContentMessage = "Não é possível adicionar um fornecedor com prazo inferir ou igual a 0.",
                ButtonDefinitions = ButtonEnum.YesNo, 
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            await msgBox.ShowAsync().ConfigureAwait(false);
        }

        // Trocar por uma "Data Validation" na text box quando digitado.
        if (!await Supply.ValidarCEP(CepMaskedTextBox.Text))
        {
            var cepMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "CEP inválido",
                ContentMessage = $"O CEP \"{CepMaskedTextBox.Text}\" digitado não existe. \nDigite outro que seja válido.",
                ButtonDefinitions = ButtonEnum.Ok, 
                Icon = MsBox.Avalonia.Enums.Icon.Warning,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
           await cepMsgBox.ShowAsync().ConfigureAwait(false);
        }
        
        Supply newSupply = new Supply(NameTextBox.Text, CnpjMaskedTextBox.Text, AutoCompleteSelectedProducts.Select(p => p.Id).ToList(), dateLimit, 
            CepMaskedTextBox.Text, AddressTextBox.Text, PhoneMaskedTextBox.Text, EmailTextBox.Text);

        var oldsupply = SupplyCtrl.FindSupply(newSupply.Cnpj);
        if (oldsupply is not null)
        {
            newSupply.Id = oldsupply.Id;
            SupplyCtrl.UpdateSupply(newSupply);
            
            // Mudar para popUp Personalizado.
            Dispatcher.UIThread.Post(async () =>
            {
                var msgbox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ContentHeader = "Fornecedor foi Editado!!",
                    ContentMessage = $"Os dados do fornecedor \"{newSupply.Name}\" foram modificados!",
                    ButtonDefinitions = ButtonEnum.Ok,
                    Icon = MsBox.Avalonia.Enums.Icon.Success,
                    CanResize = false,
                    ShowInCenter = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SystemDecorations = SystemDecorations.BorderOnly
                });
                await msgbox.ShowAsync();
            });
            return;
        }
        
        SupplyCtrl.AddNewSupply(newSupply);
        
        Dispatcher.UIThread.Post(async () =>
        {
            var msgbox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Novo Fornecedor Adicionado!",
                ContentMessage = $"O fornecedor \"{newSupply.Name}\" foi adicionado ao sistema!",
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = MsBox.Avalonia.Enums.Icon.Success,
                CanResize = false,
                ShowInCenter = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            await msgbox.ShowAsync();
            ClearTextBox(); 
        });
        
    }

    private async void CleanText_OnClick(object sender, RoutedEventArgs e)
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
        AutoCompleteSelectedProducts.Clear();
        TagContentStackPanel.Children.Clear();
        
        ProductsAutoCompleteBox.ItemsSource = Database.ProductsList.Select(p => p.Name);
        
    }
    private void ClearTextBox()
    {
        CnpjMaskedTextBox.Text = null;
        NameTextBox.Text = null;
        AddressTextBox.Text = null;
        DateLimitTextBox.Text = null;
        EmailTextBox.Text = null;
        PhoneMaskedTextBox.Text = null;
        ProductsAutoCompleteBox.Text = null;
        CepMaskedTextBox.Text = null;
    }

    private List<string> GetTextBoxes() 
        => new() {
            NameTextBox.Text,
            EmailTextBox.Text,
            AddressTextBox.Text,
            DateLimitTextBox.Text,
        };

    private void ProductsAutoCompleteBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = ProductsAutoCompleteBox.Text;
        if (keyword.LastOrDefault() != ',') return;
        
        keyword = keyword!.Replace(",", "");
        Product prod = StorageController.FindProductByNameAsync(keyword);
        if (prod is null || AutoCompleteSelectedProducts.Any(p => p.Id == prod.Id))
        {
            ProductsAutoCompleteBox.Text = ProductsAutoCompleteBox.Text!.Replace(",", "");
            return;
        }
        AutoCompleteSelectedProducts.Add(prod);
        var itemSource = ProductsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
        itemSource.Remove(prod.Name);
        ProductsAutoCompleteBox.ItemsSource = itemSource;
            
        ProductsAutoCompleteBox.Text = "";
        TagContentStackPanel.Children.Add(GenereteAutoCompleteTag(prod));
    }

    public Border GenereteAutoCompleteTag(Product product)
    {
        Label label = new() { Content = product.Name };
        Image image = new();
        WrapPanel wrapPanel = new() { Children = { label, image } };

        var border = new Border
        {
            Child = wrapPanel,
            Classes = { "AutoCompleteTag" }
        };
        border.PointerPressed += (_, _) =>
        {
            AutoCompleteSelectedProducts.Remove(product);
            TagContentStackPanel.Children.Remove(border);
            
            var itemSource = ProductsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
            itemSource.Add(product.Name);
            ProductsAutoCompleteBox.ItemsSource = itemSource;
        };
        return border;
    }
    
}