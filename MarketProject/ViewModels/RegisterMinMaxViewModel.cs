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

public class RegisterMinMaxViewModel : ViewModelBase
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


    private int _weekdaysMin;
    public int WeekdaysMin
    {
        get => _weekdaysMin;
        set
        {
            _weekdaysMin = value;
            ClearErrors(nameof(WeekdaysMax));
            if (_weekdaysMin > WeekdaysMax)
                AddError(nameof(WeekdaysMax), "Estoque máximo é inferior ao mínimo");
            else
                RemoveError(nameof(WeekdaysMax));
        }
    }
    public int WeekdaysMax { get; set; }

    private int _weekendsMin;
    public int WeekendsMin
    {
        get => _weekendsMin;
        set
        {
            _weekendsMin = value;
            ClearErrors(nameof(WeekendsMax));
            if (_weekendsMin > WeekendsMax)
                AddError(nameof(WeekendsMax), "Estoque máximo é inferior ao mínimo");
            else
                RemoveError(nameof(WeekendsMax));
        }
    }
    public int WeekendsMax { get; set; }

    private int _eventsMin;
    public int EventsMin
    {
        get => _eventsMin;
        set
        {
            _eventsMin = value;
            ClearErrors(nameof(EventsMax));
            if (_eventsMin > EventsMax)
                AddError(nameof(EventsMax), "Estoque máximo é inferior ao mínimo");
            else
                RemoveError(nameof(EventsMax));
        }
    }
    public int EventsMax { get; set; } 
}