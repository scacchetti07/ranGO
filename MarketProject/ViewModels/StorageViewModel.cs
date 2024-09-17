using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MarketProject.Models;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using MarketProject.Models;
using ReactiveUI;
using MarketProject.Models;

namespace MarketProject.ViewModels;

public class StorageViewModel : ViewModelBase
{
    
    public ObservableCollection<Product> ProductsList { get; }
    public StorageViewModel()
    {
        ProductsList = Database.ProductsList;
    }
    
    // public ProductCard ProductToCard(Product prd)
    // {
    //     return new ProductCard()
    //     {
    //         ProdName = prd.ProdName,
    //         ProdQtd = prd.ProdQtd,
    //         ProdStatus = prd.ProdStatus,
    //         ProdMin = prd.ProdMin,
    //         ProdMax = prd.ProdMax,
    //         SupplyName = prd.SupplyName,
    //         Id = prd.Id
    //     };
    // }
}