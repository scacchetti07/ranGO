using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using ExCSS;
using MarketProject.Models;
using Tmds.DBus.SourceGenerator;

namespace MarketProject.ViewModels;

public class StorageViewModel : ViewModelBase
{
    
    public ObservableCollection<Product> ProductsList { get; set; }

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