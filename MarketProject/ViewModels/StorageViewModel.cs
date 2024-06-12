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
            ProdCat = prd.ProdCat,
            ProdMin = prd.ProdMin,
            ProdMax = prd.ProdMax,
            SupName = prd.SupName,
            Id = prd.Id
        };

    }
}