using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ExCSS;
using MarketProject.Controllers;
using MarketProject.Extensions;
using MarketProject.Models;
using MarketProject.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Key = Avalonia.Remote.Protocol.Input.Key;
using SupplyCtrl = MarketProject.Controllers.SupplyController;

namespace MarketProject.Views;

public partial class SupplyAddView : Window
{
    public delegate void SupplyAddedDelegate(Supply? supply);

    public event SupplyAddedDelegate? SupplyAdded;

    public List<Product> AutoCompleteSelectedProducts { get; } = [];

    public SupplyAddView(Supply selectedSupply = null)
    {
        InitializeComponent();
        this.ResponsiveWindow();

        CepMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        CnpjMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        DateLimitTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        PhoneMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        ProductsAutoCompleteBox.AddHandler(KeyDownEvent, (sender, e) =>
        {
            if (sender is not AutoCompleteBox autoComplete)
                return;
            if (e.Key != Avalonia.Input.Key.Tab || autoComplete.Text is null || autoComplete.Text.Trim() == "")
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

        ProductsAutoCompleteBox.ItemsSource = Database.ProductsList.Select(p => p.Name);

        ProductsAutoCompleteBox.Loaded += (_, _) =>
        {
            TextBox textBox =
                ProductsAutoCompleteBox.GetTemplateChildren().Single(control => control is TextBox) as TextBox;
            textBox!.Bind(TextBox.TextProperty, new Binding()
            {
                Path = "ProductName",
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        };

        if (selectedSupply is null) return;
        AddButton.Content = "Editar";

        List<Product> products = StorageController.FindProductsFromSupply(selectedSupply);
        NameTextBox.Text = selectedSupply.Name;
        var editedCnpj = selectedSupply.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

        CnpjMaskedTextBox.Text = editedCnpj;
        CnpjMaskedTextBox.IsEnabled = false;
        foreach (var prod in products)
        {
            AutoCompleteSelectedProducts.Add(prod);
            TagContentStackPanel.Children.Add(GenereteAutoCompleteTag(prod));

            var itemSource = ProductsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
            itemSource.Remove(prod.Name);
            ProductsAutoCompleteBox.ItemsSource = itemSource;
        }

        DateLimitTextBox.Text = selectedSupply.DayLimit.ToString();
        CepMaskedTextBox.Text = selectedSupply.Cep.Replace("-", "");
        AddressTextBox.Text = selectedSupply.Adress;
        EmailTextBox.Text = selectedSupply.Email;
        PhoneMaskedTextBox.Text = selectedSupply.Phone!.Replace(" ", "");
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
                ContentMessage =
                    "Ainda existem dados escritos nos campos de cadastro,\nQuer realmente sair do cadastro?",
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
        if (DateLimitTextBox.Text == null) return;

        int dateLimit = Convert.ToInt32(DateLimitTextBox.Text);
        List<string> textBoxes = GetTextBoxes();
        if (textBoxes.Any(txt => string.IsNullOrEmpty(txt))) return;
        if (!AutoCompleteSelectedProducts.Any()) return;

        try
        {
            if (int.Parse(DateLimitTextBox.Text) <= 0)
                throw new Exception("Não é possível adicionar um fornecedor com prazo inferir ou igual a zero.");
            if (AutoCompleteSelectedProducts.Count == 0)
                throw new Exception("Não existe produtos adicionado na lista deste fornecedor. Adicione!");

            string cnpj = CnpjMaskedTextBox.Text;
            if (cnpj.Contains(','))
                cnpj = CnpjMaskedTextBox.Text.Replace(',', '.');


            Supply newSupply = new Supply(NameTextBox.Text, cnpj,
                AutoCompleteSelectedProducts.Select(p => p.Id).ToList(), dateLimit,
                CepMaskedTextBox.Text, AddressTextBox.Text, PhoneMaskedTextBox.Text, EmailTextBox.Text);

            var oldsupply = SupplyCtrl.FindSupplyByCnpj(newSupply.Cnpj);
            if (oldsupply is not null)
            {
                newSupply.Id = oldsupply.Id;
                SupplyCtrl.UpdateSupply(newSupply);
                SupplyAdded?.Invoke(newSupply);
                return;
            }

            if (await Supply.ValidarCEP(CepMaskedTextBox.Text) is null)
                throw new Exception($"CEP informado é inválido e não existe.");
            if (await Supply.ConsultaCNPJ(CnpjMaskedTextBox.Text) is null)
                throw new Exception($"CNPJ informado é inválido e não existe.");
            SupplyCtrl.AddNewSupply(newSupply);
            SupplyAdded?.Invoke(newSupply);
            ClearTextBox();
            AutoCompleteSelectedProducts.Clear();
            TagContentStackPanel.Children.Clear();
        }
        catch (Exception ex)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Erro ao cadastrar fornecedor",
                ContentMessage = ex.Message,
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = MsBox.Avalonia.Enums.Icon.Error,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            await msgBox.ShowAsync().ConfigureAwait(false);
        }
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
        CepMaskedTextBox.Text = null;
    }

    private List<string> GetTextBoxes()
        => new()
        {
            NameTextBox.Text,
            EmailTextBox.Text,
            AddressTextBox.Text,
            DateLimitTextBox.Text,
        };

    private void ProductsAutoCompleteBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Avalonia.Input.Key.Enter) return;

        Product prod = StorageController.FindProductByNameAsync(ProductsAutoCompleteBox.Text);
        if (prod is null || AutoCompleteSelectedProducts.Any(p => p.Id == prod.Id)) return;

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
        StackPanel stackPanel = new() { Children = { label, image } };

        var border = new Border
        {
            Child = stackPanel,
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


    private async void CepMaskedTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = CepMaskedTextBox.Text;

        if (keyword.Contains('_')) return;

        var cepContent = await Supply.ValidarCEP(keyword);
        if (cepContent is null) return;

        AddressTextBox.Text = $"{cepContent.logradouro} - {cepContent.localidade}";
    }

    private async void CnpjMaskedTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (CnpjMaskedTextBox.IsEnabled is false) return;
        var keyword = CnpjMaskedTextBox.Text;
        if (keyword.Contains('_')) return;
        try
        {
            var cnpjContent = await Supply.ConsultaCNPJ(keyword);
            if (cnpjContent is null) return;

            NameTextBox.Text = cnpjContent?.fantasia;
            CepMaskedTextBox.Text = cnpjContent.cep.Replace(".", "");
            AddressTextBox.Text = $"{cnpjContent.logradouro} - {cnpjContent.uf}";
            PhoneMaskedTextBox.Text = cnpjContent.telefone;
            EmailTextBox.Text = cnpjContent.email;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void PhoneType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PhoneMaskedTextBox == null) return;
        
        PhoneMaskedTextBox.Mask = PhoneType.SelectedIndex == 0 
            ? "(00) 0000-0000" 
            : "(00) 00000-0000";
    }
}