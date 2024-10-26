using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.Views;
using ReactiveUI;
using static MarketProject.Models.Supply;

namespace MarketProject.ViewModels;

public class SupplyAddViewModel : ViewModelBase
{
    private string _cnpj;
    public string Cnpj
    {
        get => _cnpj;
        set
        {
            _cnpj = value;
            ClearErrors(nameof(Cnpj));
            if (Cnpj.Contains('_'))
                AddError(nameof(Cnpj), "CNPJ está incompleto e é obrigatório!");
        }
    }
    private string _cep;
    public string Cep
    {
        get => _cep;
        set
        {
            _cep = value;
            ClearErrors(nameof(Cep));
            if (Cep.Contains('_'))
                AddError(nameof(Cep), "CEP está incompleto e é obrigatório!");
            else
                RemoveError(nameof(Email));
        }
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set
        {
            _email = value;
            ClearErrors(nameof(Email));
            if (!_email.Contains('@') || !_email.Contains('.'))
                AddError(nameof(Email), "Email deve conter '@' e '.'");
            else
                RemoveError(nameof(Email));
        }
    }

    
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