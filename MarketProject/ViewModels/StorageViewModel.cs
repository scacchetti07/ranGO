using System.Collections.ObjectModel;
using System.Windows.Input;
using MarketProject.Models;
using System.Reactive.Linq;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class StorageViewModel : ViewModelBase
{
    public ObservableCollection<Product> Product { get; set; }
    
    public ICommand OpenProductRegisterButton { get; }
    
    public Interaction<ProdRegisterViewModel, StorageViewModel> ShowDialog { get; }
    public StorageViewModel()
    {
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