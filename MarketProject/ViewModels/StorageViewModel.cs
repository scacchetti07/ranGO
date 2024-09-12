using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MarketProject.Models;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using MarketProject.Views;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class StorageViewModel : ViewModelBase
{
    public ObservableCollection<Product> ProductsList { get; }

    public StorageViewModel()
    {
        ProductsList = new ObservableCollection<Product>(new List<Product>()
        {
            new Product(1234, "Arroz", new Supply("Fornecedor Pica"), 14.5m, "Unidade", new Range(12,30), new Range(10, 40), new Range(11, 50))
        });
    }
    
    public void AddProduct(Product product)
    {
        ProductsList?.Add(product);
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