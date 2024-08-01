using System.Collections.Generic;
using System.Collections.ObjectModel;
using MarketProject.Models;

namespace MarketProject.ViewModels;

public class StorageViewModel : ViewModelBase
{
    public ObservableCollection<Product> Product { get; set; }
    public StorageViewModel()
    {
        //Product = new ObservableCollection<Product>(products);
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