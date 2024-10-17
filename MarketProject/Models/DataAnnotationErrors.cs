using System;
using cm = System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MongoDB.Driver.Linq;
using ReactiveUI;

namespace MarketProject.Models;

public class DataAnnotationErrors : ReactiveObject
{
    private DateTime _productValidaty;

    [cm.Compare(nameof(DateTime), ErrorMessage = "Data fornecida é superior ao dia de hoje")]
    public DateTime ProductValidaty
    {
        get => _productValidaty;
        set => this.RaiseAndSetIfChanged(ref _productValidaty, value);
    }

    public static string ValidationMessage(string message)
        => message;
}