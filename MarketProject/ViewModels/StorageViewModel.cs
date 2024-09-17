using System.Collections.ObjectModel;
using Avalonia.Controls;
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