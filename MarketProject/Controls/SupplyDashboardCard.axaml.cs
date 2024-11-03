using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Controls;

public partial class SupplyDashboardCard : UserControl
{
    public static readonly StyledProperty<Supply> CurrentSupplyProperty =
        AvaloniaProperty.Register<SupplyDashboardCard, Supply>(nameof(CurrentSupply));
    public Supply CurrentSupply
    {
        get => GetValue(CurrentSupplyProperty);
        set => SetValue(CurrentSupplyProperty, value);
    }


    public SupplyDashboardCard()
    {
        InitializeComponent();
        CurrentSupplyProperty.Changed.AddClassHandler<SupplyDashboardCard>((_, _) => UpdateDashboardCard());
        
        if (CurrentSupply is not null) return;
        
        MoreInfoAboutSupplyButton.IsVisible = false;
        WhatsappButton.IsVisible = false;
        IsSupplyDefinied.IsVisible = true;
    }

    private void UpdateDashboardCard()
    {
        var name = new string(CurrentSupply.Name.ToUpper().Take(10).ToArray());
        DashboardCardTitle.Text = $"CHEGADA DO {name}";
        DashboardCardMainContent.Text = $"{CurrentSupply.DayLimit} DIAS";
        
        MoreInfoAboutSupplyButton.IsVisible = true;
        WhatsappButton.IsVisible = true;
        IsSupplyDefinied.IsVisible = false;
    }
    
    private void WhatsappButton_OnClick(object sender, RoutedEventArgs e)
    {
        var phone = CurrentSupply.Phone.Replace("-", "").Replace("(", "").Replace(")", "").Replace("_", "").Trim();
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = $"https://api.whatsapp.com/send?phone=55{phone.Replace(" ", "")}&text=Olá {CurrentSupply.Name.ToLower()}, gostaria de saber como está o trajeto do meu pedido!",
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    private async void MoreInfoAboutSupplyButton_OnClick(object sender, RoutedEventArgs e)
    {
        SupplyAddView editSupply = new(CurrentSupply)
        {
            Title = "Cadastro de Fornecedores",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        await editSupply.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!);
        editSupply.IsEnabled = false;
    }
}