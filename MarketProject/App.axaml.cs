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

    private ServiceProvider? _provider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        var collection = new ServiceCollection();
        collection.AddSingleton<Database>();
        collection.AddSingleton<HomeViewModel>();

        var services = collection.BuildServiceProvider();
        _provider = services;
        this.Resources[typeof(ServiceProvider)] = services;
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
                _provider?.GetRequiredService<Database>().Serialize();
            };
            desktop.Startup += (_, _) =>
            {
                _provider?.GetRequiredService<Database>().Deserialize();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}