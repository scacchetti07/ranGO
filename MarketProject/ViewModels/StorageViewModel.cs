using MarketProject.Controls;
using MarketProject.Models;
using Avalonia;

namespace MarketProject.ViewModels;

public class StorageViewModel : ViewModelBase
{
    // Método que transforma o produto em ProductCard 
    public ProductCard ProductToCard(Product prd)
    {
        return new ProductCard()
        {
            ProdName = prd.ProdName,
            ProdQtd = prd.ProdQtd,
            ProdStatus = prd.ProdCat,
            ProdMin = prd.ProdMin,
            ProdMax = prd.ProdMax,
            SupplyName = prd.SupName,
            Id = prd.Id
        };

    }
}