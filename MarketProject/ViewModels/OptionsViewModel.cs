using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Mime;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using MarketProject.Helpers;
using ReactiveUI;
using MarketProject.ViewModels;
using MarketProject.Views;

namespace MarketProject.ViewModels;

public class OptionsViewModel : ReactiveObject
{
    private HomeViewModel _homeViewModel = new();
    
    public OptionsViewModel()
    {
        // Definindo a lista de temas e o padrão quando for iniciado
        Themes = new ObservableCollection<string> { _dark, _light } ;
        DefaultThemes();
    }
    
    private const string _light = "Claro";
    private const string _dark = "Escuro";
    private string _currentAppTheme;
    
    public string CurrentAppTheme
    {
        get => _currentAppTheme;
        set
        {
            var theme = this.RaiseAndSetIfChanged(ref _currentAppTheme, value);
            if (GetTheme(theme) == null) return;
            Application.Current.RequestedThemeVariant = GetTheme(theme);
        }
    }
    public void DefaultThemes()
    {
        CurrentAppTheme = _dark;
    }
    
    public ObservableCollection<string> Themes { get; }

    public ThemeVariant GetTheme(string value)
    {
        return value switch
        {
            _light => ThemeVariant.Light,
            _dark => ThemeVariant.Dark,
            _ => null
        };
    }
    
}

