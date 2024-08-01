using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Controls;
using MarketProject.Models;
using ReactiveUI;
using Tmds.DBus.Protocol;

namespace MarketProject.ViewModels;

public class RegisterMinMaxViewModel : ViewModelBase
{
    public string SectionTitle { get; set; }

    public int WeekdaysMin { get; set; }
    public int WeekdaysMax { get; set; }
    
    public int WeekendsMin { get; set; }
    public int WeekendsMax { get; set; }
    
    public int EventsMin { get; set; }
    public int EventsMax { get; set; }
}