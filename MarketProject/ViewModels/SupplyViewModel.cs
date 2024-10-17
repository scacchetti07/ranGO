using System.Collections.Generic;
using System.Linq;
using MarketProject.Controllers;
using MarketProject.Models;

namespace MarketProject.ViewModels;

public class SupplyViewModel : ViewModelBase
{
    public static SupplyDataGrid SuppliesToDataGrid(Supply supply)
    {
        List<Product> supplyProducts = StorageController.FindProductsFromSupply(supply);
        var products = supplyProducts.Any()
            ? supplyProducts.Take(2).Select(p => p.Name).Aggregate((sum, current) => sum + ", " + current)+"..."
            : string.Empty;
        
        return new SupplyDataGrid(supply.Cnpj, supply.Name, supply.Phone, supply.Cep,
            products, $"{supply.DayLimit} dias");
    }
}

public record SupplyDataGrid(string Cnpj, string Name, string Phone, string Cep, string Products, string Date);