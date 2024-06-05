using System;
using System.Collections.Generic;
using MarketProject.Models;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public Database Database { get; init; }

    public HomeViewModel(Database database)
    {
        Database = database;
    }
    
}