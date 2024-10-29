using MarketProject.Controllers;

namespace MarketProject.ViewModels;

public class ManageFoodViewModel : ViewModelBase
{
    private string? _productName;
    public string? ProductName
    {
        get => _productName;
        set
        {
            _productName = value;
            ClearErrors(nameof(ProductName));
            if (ProductName?.Trim() != "" && StorageController.FindProductByNameAsync(ProductName) is null)
                AddError(nameof(ProductName), "Produto digitado não no estoque!");
            else if (!string.IsNullOrEmpty(ProductName))
                AddError(nameof(ProductName), "Produto não foi adicionado. Clique 'Enter'");
            else
                RemoveError(nameof(ProductName));
        }
    }
}