using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MarketProject.ViewModels;
using Microsoft.Extensions.DependencyInjection; // biblioteca da injeção de dependência
using MarketProject.Extensions;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Views;

public partial class LoginPage : Window
{
    private LoginPageViewModel ViewModel => DataContext as LoginPageViewModel;
    public LoginPage()
    {
        InitializeComponent();
        this.ResponsiveWindow();
    }

    private void btnlogin(object? sender, RoutedEventArgs e)
    {
        bool res = ViewModel.VerifLogin(txtUser.Text!, txtPass.Text!);
        if (res)
        {
            var homeView = new HomeView()
            {
                DataContext = ((ServiceProvider)this.FindResource(typeof(ServiceProvider))!)
                    .GetRequiredService<HomeViewModel>()
            };
            homeView.Show();
            Close(); return;
        }
        
        var msgbox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams {
            ContentHeader = "Credenciais Incorretas!!",
            ContentMessage = $"Os dados de login fornecedidas estão incorretos! Tente Novamente",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = MsBox.Avalonia.Enums.Icon.Error,
            CanResize = false,
            ShowInCenter = true,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        msgbox.ShowAsync();
        
        txtPass.Text = string.Empty;
        txtUser.Text = string.Empty;
    }
}