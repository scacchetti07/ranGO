using System.Linq;
using MarketProject.Models;

namespace MarketProject.ViewModels;

public class SupplyViewModel : ViewModelBase
{
    public static SupplyDataGrid SuppliesToDataGrid(Supply supply)
    {
        var products = supply.Products.Any()
            ? supply.Products.Select(p => p.Name).Aggregate((sum, current) => sum + ", " + current)
            : string.Empty;
        
        return new SupplyDataGrid(supply.Cnpj, supply.Name, supply.Phone, supply.Cep,
            products, supply.DayLimit);
    }
}

public record SupplyDataGrid(string Cnpj, string Name, string Phone, string Cep, string Products, int Date);