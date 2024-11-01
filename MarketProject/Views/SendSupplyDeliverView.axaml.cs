using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Views;

public partial class SendSupplyDeliverView : Window
{
    public delegate void SendSupplyToDeliver(Supply? supply);

    public event SendSupplyToDeliver SendSupply;
    
    public SendSupplyDeliverView()
    {
        InitializeComponent();
        SupplyNameAutoCompleteBox.AddHandler(KeyDownEvent, (sender, e) =>
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

        SupplyNameAutoCompleteBox.ItemsSource = Database.SupplyList.Select(s => s.Name);
    }

    private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
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

    private List<string> GetTextBoxes() =>
    [
        SupplyNameAutoCompleteBox.Text,
        DeliverTextBox.Text,
        CnpjTextBox.Text
    ];

    private void DeliverSupplyButton_OnClick(object sender, RoutedEventArgs e)
    {
        List<string> textBoxes = GetTextBoxes();
        if (textBoxes.TrueForAll(t => t is null)) return;

        var selectedSupply = SupplyController.FindSupplyByName(SupplyNameAutoCompleteBox.Text);
        if (selectedSupply is null)
        {
            SendSupply?.Invoke(null);
            return;
        }

        selectedSupply.InDeliver = true;
        SupplyController.UpdateSupply(selectedSupply);
        SendSupply?.Invoke(selectedSupply);
        Close();    
    }

    private void SupplyNameAutoCompleteBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = SupplyNameAutoCompleteBox.Text;
        var selectedSupply = SupplyController.FindSupplyByName(keyword);
        if (selectedSupply is null) return;
        
        DeliverTextBox.Text = $"{selectedSupply.DayLimit} dias";
        CnpjTextBox.Text = selectedSupply.Cnpj;
    }
}