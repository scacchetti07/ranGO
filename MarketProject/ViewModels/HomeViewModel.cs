using System;
using System.Collections.Generic;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MarketProject.Models;
using ReactiveUI;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using MarketProject.Helpers;


namespace MarketProject.ViewModels;

public class HomeViewModel : ReactiveObject
{
    private Bitmap? _currentLogoTheme;
    public Bitmap? CurrentLogoTheme
    {
        get => _currentLogoTheme;
        set => this.RaiseAndSetIfChanged(ref _currentLogoTheme, value);
    }

    public old_Database OldDatabase { get; init; }
    // public getter permite que o database seja lido por todo o sistema
    // public init permite que o database seja iniciada e executado por todo o sistema 
    // (mas não modificado)
    
    public HomeViewModel(old_Database oldDatabase)
    {
        OldDatabase = oldDatabase;
        CurrentLogoTheme = ImageHelper.LoadFromResource(new Uri($"avares://MarketProject/Assets/ranGoLight.png"));
    }

    public HomeViewModel()
    { }
}