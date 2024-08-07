using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MarketProject.Models;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class StorageViewModel : ViewModelBase
{
    public ObservableCollection<Product> Product { get; }
    
   // public ICommand OpenProductRegisterButton { get; }
    
    //public Interaction<ProdRegisterViewModel, StorageViewModel> ShowDialog { get; }
    public StorageViewModel()
    {
        UpdateStorage();
        //Product = new ObservableCollection<Product>(list);
        // ShowDialog = new Interaction<ProdRegisterViewModel, StorageViewModel>();
        //
        // OpenProductRegisterButton = ReactiveCommand.CreateFromTask(async () =>
        // {
        //     var register = new ProdRegisterViewModel();
        //
        //     var resp = await ShowDialog.Handle(register);
        // });
        
        //Product = new ObservableCollection<Product>(products);
    }

    public void UpdateStorage(params Product[]? product )
    {
        //var productsList = new List<Product>();
        if (Product is null) return;
        //productsList.Add(product);
        foreach (var prod in product)
        {
            Product.Add(new Product(prod.GTIN, prod.ProdName, prod.Supply, prod.Price, 
                prod.Unit, prod.Weekday, prod.Weekends, prod.Events, prod.Description, 
                prod.ProdTotal, prod.Id));
            Console.WriteLine(Product);   
        }
           
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