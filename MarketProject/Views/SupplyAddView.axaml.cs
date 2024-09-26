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
using MarketProject.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using SupplyCtrl = MarketProject.Controllers.SupplyController;

namespace MarketProject.Views;

public partial class SupplyAddView : Window
{
    public SupplyAddView()
    {
        InitializeComponent();
        
        CepMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        CnpjMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        DateLimitTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        PhoneMaskedTextBox.AddHandler(TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
    }
    
    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }

    private async void ReturnButton_OnClick(object sender, RoutedEventArgs e)
    {
        List<string> textBoxes = GetTextBoxes();
        if (textBoxes.TrueForAll(txt => string.IsNullOrEmpty(txt)))
        {
            Close();
            return;
        }
        
        var checkMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Dados ainda digitados.",
            ContentMessage = "Ainda existem dados escritos nos campos de cadastro,\nDeixe-os todos em branco para retornar a tela inicial.",
            ButtonDefinitions = ButtonEnum.Ok, 
            Icon = MsBox.Avalonia.Enums.Icon.Info,
            CanResize = false,
            ShowInCenter = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        await checkMsgBox.ShowAsync().ConfigureAwait(false);
        
    }

    private async void AddSupplyButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Verificar se cnpj digitado é real após implementar a API do gov.br
        // Aplicar "Data Validation" Nos campos de CNPJ e CEP caso estejam incorretos.
        
        
        int dateLimit = Convert.ToInt32(DateLimitTextBox.Text);
        
        List<string> textBoxes = GetTextBoxes();
        
        // if (textBoxes.Any(txt => string.IsNullOrEmpty(txt))) return;

        // if (int.Parse(DateLimitTextBox.Text) <= 0)
        // {
        //     var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        //     {
        //         ContentHeader = "Prazo inferior ou igual a 0",
        //         ContentMessage = "Não é possível adicionar um fornecedor com prazo inferir ou igual a 0.",
        //         ButtonDefinitions = ButtonEnum.YesNo, 
        //         Icon = MsBox.Avalonia.Enums.Icon.Info,
        //         CanResize = false,
        //         ShowInCenter = true,
        //         SizeToContent = SizeToContent.WidthAndHeight,
        //         WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //         SystemDecorations = SystemDecorations.BorderOnly
        //     });
        //     await msgBox.ShowAsync().ConfigureAwait(false);
        // }

        // Trocar por uma "Data Validation" na text box quando digitado.
        // if (!await ValidarCEP(CepMaskedTextBox.Text))
        // {
        //     var cepMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        //     {
        //         ContentHeader = "CEP inválido",
        //         ContentMessage = $"O CEP \"{CepMaskedTextBox.Text}\" digitado não existe. \nDigite outro que seja válido.",
        //         ButtonDefinitions = ButtonEnum.Ok, 
        //         Icon = MsBox.Avalonia.Enums.Icon.Warning,
        //         ShowInCenter = true,
        //         SizeToContent = SizeToContent.WidthAndHeight,
        //         WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //         SystemDecorations = SystemDecorations.BorderOnly
        //     });
        //    await cepMsgBox.ShowAsync().ConfigureAwait(false);
        // }
        
        var confirmMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Confirmar novo Fornecedor",
            ContentMessage = $"Você quer realmente adicionar este fornecedor ao sistema?",
            ButtonDefinitions = ButtonEnum.YesNo, 
            Icon = MsBox.Avalonia.Enums.Icon.Info,
            CanResize = false,
            ShowInCenter = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        var result = await confirmMsgBox.ShowAsync();

        if (result == ButtonResult.No) return;
        
        // Supply newSupply = new Supply(NameTextBox.Text, CnpjMaskedTextBox.Text, new List<Product>(), dateLimit, 
        //     CepMaskedTextBox.Text, AddressTextBox.Text, PhoneMaskedTextBox.Text, EmailTextBox.Text);

        var newSupply = new Supply("Alemanha fornecedor", "20.353.540/1111-10", new List<Product>(), 10,
            "13063-442", "Rua King", "(11) 92222-9999", "emailfornecedor@email.com");
        
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
            ProductsAutoCompleteBox.Text
        };
    
    private async Task<bool> ValidarCEP(string cep)
    {
        // Remove o hífen do CEP, se existir
        cep = cep.Replace("-", "");
        
        string url = $"https://viacep.com.br/ws/{cep}/json/";

        using HttpClient client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return false;
                
            var conteudo = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            // Verifica se o conteúdo contém a chave "erro", que indica que o CEP não foi encontrado
            return !conteudo.Contains("\"erro\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao consultar a API ViaCEP: {ex.Message}");
            return false;
        }
    }
}