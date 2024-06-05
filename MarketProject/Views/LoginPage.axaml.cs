using System;
using System.Runtime.InteropServices.JavaScript;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MarketProject.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MarketProject.Views;

public partial class LoginPage : Window
{
    private LoginPageViewModel ViewModel => DataContext as LoginPageViewModel;
    
    public LoginPage()
    {
        InitializeComponent();
    }

    private void btnlogin(object? sender, RoutedEventArgs e)
    {
        string res = ViewModel.VerifLogin(txtUser.Text!, txtPass.Text!);
        if (res == null)
        { 
            new HomeView()
            {
                DataContext = ((ServiceProvider)this.FindResource(typeof(ServiceProvider))!).GetRequiredService<HomeViewModel>()
            }.Show();
            Close();
        }
        else
        {
            new PopUpErrorView(res).Show();
            txtPass.Text = string.Empty;
            txtUser.Text = string.Empty;
        }
    }
}