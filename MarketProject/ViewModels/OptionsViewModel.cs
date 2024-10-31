using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using Markdown.Avalonia.Utils;
using ReactiveUI;
using MarketProject.ViewModels;
using MarketProject.Views;

namespace MarketProject.ViewModels;

public class OptionsViewModel : ViewModelBase
{
    public ICommand AccessWebSite { get; }
    public ICommand SentAMailTo { get; }

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
    public OptionsViewModel()
    {
        // Definindo a lista de temas e o padrão quando for iniciado
        Themes = new ObservableCollection<string> { _dark, _light } ;
        DefaultThemes();

        AccessWebSite = ReactiveCommand.Create(() =>
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://pietromauergodoy.github.io/ranGO_WebSite/",
                UseShellExecute = true
            };
            Process.Start(psi);
        });

        SentAMailTo = ReactiveCommand.Create(SentEmail);
    }
    public void DefaultThemes()
    {
        CurrentAppTheme = _dark;
    }

    private void SentEmail()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "mailto:rangoemp@gmail.com",
            UseShellExecute = true
        };
        Process.Start(psi);
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

