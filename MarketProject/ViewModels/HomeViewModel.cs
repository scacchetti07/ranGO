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

namespace MarketProject.ViewModels;

public class HomeViewModel : ReactiveObject
{
    public Database Database { get; init; }
    
    // public getter permite que o database seja lido por todo o sistema
    // public init permite que o database seja iniciada e executado por todo o sistema 
    // (mas não modificado)

    public HomeViewModel()
    { }

    public HomeViewModel(Database database)
    {
        Database = database;
    }

}