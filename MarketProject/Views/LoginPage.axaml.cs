using System;
using System.Runtime.InteropServices.JavaScript;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MarketProject.ViewModels;
using Microsoft.Extensions.DependencyInjection; // biblioteca da injeção de dependência

namespace MarketProject.Views;

public partial class LoginPage : Window
{
    // Instância que permite as funções da LoginPageViewModel serem chamadas   
    private LoginPageViewModel ViewModel => DataContext as LoginPageViewModel;
    
    public LoginPage()
    {
        InitializeComponent();
    }

    private void btnlogin(object? sender, RoutedEventArgs e)
    {
        // res é a string retornada pela verificação feita na função veriflogin
        string res = ViewModel.VerifLogin(txtUser.Text!, txtPass.Text!);
        if (res == null)
        { 
            
            // Toda vez que a home view for instanciada,
            // Os serviços declarados no provider são injetados no DataContext da login page,
            // automaticamente adicionando o banco json e as funções da Home no sistema.
            // fazendo isso, permite que no home view seja possível acessar o banco json e todas funções do sistema.
            new HomeView()
            {
                DataContext = ((ServiceProvider)this.FindResource(typeof(ServiceProvider))!).GetRequiredService<HomeViewModel>()
            }.Show();
            Close();
        }
        else
        {
            // Caso as credenciais estiverem incorreta, uma UserControl 
            // é aberta e enviado como parâmetro a mensagem retornada na função Veriflogin
            new PopUpErrorView(res).Show();
            txtPass.Text = string.Empty;
            txtUser.Text = string.Empty;
        }
    }
}