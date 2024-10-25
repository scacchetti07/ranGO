using System.Collections.Generic;
using Avalonia.Interactivity;
using MarketProject.Controllers;
using MarketProject.Models;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class ProductAddViewModel : ViewModelBase
{
    private string? _supplyName;
    public string? SupplyName
    {
        get => _supplyName;
        set
        {
            _supplyName = value;
            ClearErrors(nameof(SupplyName));
            if (SupplyName?.Trim() != "" && SupplyController.FindSupplyByName(SupplyName) is null)
                AddError(nameof(SupplyName), "Fornecedor digitado não existe no estoque!");
            else
                RemoveError(nameof(SupplyName));
        }
    }
    
}