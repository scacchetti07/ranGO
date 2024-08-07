using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MarketProject.ViewModels;
using MarketProject.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using MarketProject.Models;

namespace MarketProject;

public partial class App : Application
{

    private ServiceProvider _provider; // delcarando a váriavel de serviço
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        var collection = new ServiceCollection();
        collection.AddSingleton<old_Database>(); // Declarado o serviço "DataBase" no provider
        collection.AddSingleton<HomeViewModel>(); // Declardo o serviço "HomeViewModel" no provider
      
        var services = collection.BuildServiceProvider(); // Implementando os serviços por meio do provider
        _provider = services;
        this.Resources[typeof(ServiceProvider)] = services; // Recebe os serviõs estabelecidos pelo provider na aplicação 
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new LoginPage
            {
                DataContext = new LoginPageViewModel(),
            };
            desktop.Exit += (_, _) =>
            {
                _provider?.GetRequiredService<old_Database>().Serialize();
            };
            desktop.Startup += (_, _) =>
            {
                _provider?.GetRequiredService<old_Database>().Deserialize();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}