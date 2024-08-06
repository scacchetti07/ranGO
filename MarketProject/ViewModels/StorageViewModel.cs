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
    public ObservableCollection<Product> Product { get; set; }
    
   // public ICommand OpenProductRegisterButton { get; }
    
    public Interaction<ProdRegisterViewModel, StorageViewModel> ShowDialog { get; }
    public StorageViewModel()
    {
        var list = UpdateStorage();
        if (list is null) return;
        Product = new ObservableCollection<Product>(list);
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

    public List<Product> UpdateStorage(Product? product = null)
    {
        var productsList = new List<Product>();
        if (Product is null) return null;
        productsList.Add(product);
        foreach (var prod in Product)
        {
            productsList.Add(new Product(prod.GTIN, prod.ProdName, prod.Supply, prod.Price, 
                prod.Unit, prod.Weekday, prod.Weekends, prod.Events, prod.Description, 
                prod.ProdTotal, prod.Id));
        }
        return productsList;

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