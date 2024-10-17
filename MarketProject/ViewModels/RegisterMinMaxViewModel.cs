using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using MarketProject.Models;
using ReactiveUI;
using Tmds.DBus.Protocol;

namespace MarketProject.ViewModels;

public class RegisterMinMaxViewModel : ReactiveObject
{
    public RegisterMinMaxViewModel()
    {
        var theme = Application.Current?.RequestedThemeVariant;
        ChangeBackground = GetColor(theme?.ToString());
    }
    private ISolidColorBrush _changeBackground;
    public ISolidColorBrush ChangeBackground
    {
        get => _changeBackground;
        set => this.RaiseAndSetIfChanged(ref _changeBackground, value);
    }

    private ISolidColorBrush GetColor(string value)
    {
        switch (value)
        {
            case "Light":
                return new SolidColorBrush(Color.Parse("#D6DBE2"));
            case "Dark":
                return new SolidColorBrush(Color.Parse("#1C1F2B"));
            default:
                return null;
        }
    }

    private string _sectionTitle;

    public string SectionTitle
    {
        get => _sectionTitle;
        set => this.RaiseAndSetIfChanged(ref _sectionTitle, value);
    } 

    public int WeekdaysMin { get; set; }
    public int WeekdaysMax { get; set; }
    
    public int WeekendsMin { get; set; }
    public int WeekendsMax { get; set; }
    
    public int EventsMin { get; set; }
    public int EventsMax { get; set; }
}