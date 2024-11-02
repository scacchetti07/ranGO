using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using MarketProject.Extensions;
using MarketProject.Models;
using MarketProject.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Views;

public partial class HomeView : Window
{
    private Button? _selectedButton;

    // Chamando as funcionalidades do HomeViewModel
    private HomeViewModel ViewModel => DataContext as HomeViewModel;
    
    public HomeView()
    {
        InitializeComponent();
        Application.Current.ActualThemeVariantChanged += (_, _) =>
        {
            var theme = Application.Current.RequestedThemeVariant.ToString();
            switch (theme)
            {
                case "Light":
                    Classes.Add("Light");
                    break;
                case "Dark":
                    Classes.Remove("Light");
                    break;
            }
        };
        this.ResponsiveWindow();
    }

    // Método que verifica se a variável do botão selecionado é nula,
    // Caso for verdade, ele retorna nada, se não for, ele remove o estilo de selecionado
    private void unSelectButton()
    {
        _selectedButton?.Classes.Remove("isSelected");
    }

    // Método que adiciona uma seleção visual no botão clicado
    private void selectButton(Button? btn)
    {
        _selectedButton = btn;
        _selectedButton.Classes.Add("isSelected");
    }

    // Método que verifica se aquele botão 
    private void toggleSelectedButton(Button? btn)
    {
        unSelectButton(); // ele remove o estilo caso a variável _selectedButton tiver algum valor
        selectButton(btn); // ele adiciona o estilo de selecionado ao botão clicado
    }
    
    private void btnDashboard(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 1;
        toggleSelectedButton(sender as Button);
    }
    private void btnStorage(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 2;
        toggleSelectedButton(sender as Button);
    }
    private void BtnSupply(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 3;
        toggleSelectedButton(sender as Button);
    }

    private void btnOrder(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 4;
        toggleSelectedButton(sender as Button);
    }

    private void btnSettings(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 5;
        toggleSelectedButton(sender as Button);
    }

    private async void btnExit(object sender, RoutedEventArgs e)
    {
        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Encerrar sessão e Sair do Sistema?",
            ContentMessage = "Você tem certeza que realmente quer encerrar a sessão?",
            ButtonDefinitions = ButtonEnum.YesNo, 
            Icon = MsBox.Avalonia.Enums.Icon.Warning,
            CanResize = false,
            ShowInCenter = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        var result = await msgBox.ShowWindowDialogAsync(this);
        if (result == ButtonResult.No) return;
        Environment.Exit(0);
    }

    private void OpenDashboardToBegin_OnClick(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 1;
        toggleSelectedButton(DashboardButton);
    }
}