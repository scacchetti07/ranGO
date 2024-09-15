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
using Avalonia.Svg.Skia;

namespace MarketProject.ViewModels;

public class HomeViewModel : ReactiveObject
{
    private Bitmap? _currentLogoTheme;

    public Bitmap? CurrentLogoTheme
    {
        get => _currentLogoTheme;
        set => this.RaiseAndSetIfChanged(ref _currentLogoTheme, value);
    }

    private string _currentCss;

    public string CurrentCss
    {
        get => _currentCss;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentCss, value);
            var themeCss = Application.Current?.RequestedThemeVariant?.ToString();
            _currentCss = GetColorCss(themeCss);
        }
    }

    private string GetColorCss(string theme)
    {
        switch (theme)
        {
            case "Dark":
                return ".background { fill: #DBDBDA }";
            case "Light":
                return ".background { fill: #0B0E18 }";
            default:
                return null;
        }
    }
    
    public old_Database OldDatabase { get; init; }
    
    // public getter permite que o database seja lido por todo o sistema
    // public init permite que o database seja iniciada e executado por todo o sistema 
    // (mas não modificado)

    public HomeViewModel()
    { }

    public HomeViewModel(old_Database oldDatabase)
    {
        OldDatabase = oldDatabase;
        CurrentLogoTheme = ImageHelper.LoadFromResource(new Uri($"avares://MarketProject/Assets/ranGoLight.png"));
    }

}