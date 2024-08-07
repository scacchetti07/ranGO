using System;
using System.Collections.Generic;
using MarketProject.Models;
using MarketProject.Views;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public old_Database OldDatabase { get; init; }
    // public getter permite que o database seja lido por todo o sistema
    // public init permite que o database seja iniciada e executado por todo o sistema 
    // (mas não modificado)
    
    public HomeViewModel(old_Database oldDatabase)
    {
        OldDatabase = oldDatabase;
    }
    
}