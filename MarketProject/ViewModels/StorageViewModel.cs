using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.Views;
using ReactiveUI;
using Timer = System.Timers.Timer;

namespace MarketProject.ViewModels;

public class StorageViewModel : ReactiveObject
{
    private static bool _productWasAdded;

    public static bool ProductWasAdded
    {
        get => _productWasAdded;
        set
        {
            if (!value) return;
                _productWasAdded = value;
        }
    }

    public static ProductDataGrid ProductToDataGrid(Product prod, MinMaxOptions options)
    {
        int min = 0, max = 0;
        switch (options)
        {
            case MinMaxOptions.Weekdays:
                min = prod.Weekdays.Min;
                max = prod.Weekdays.Max;
                break;
            case MinMaxOptions.Weekends:
                min = prod.Weekends.Min;
                max = prod.Weekends.Max;
                break;
            case MinMaxOptions.Events:
                min = prod.Events.Min;
                max = prod.Events.Max;
                break;
        }

        string supplyName = SupplyController.GetSupplyNameByProduct(prod);
        ProductWasAdded = true;    
        
        return new ProductDataGrid(prod.Gtin, prod.Name, prod.Total, supplyName, min, max);
    }
}

public record ProductDataGrid(long Gtin, string Name, int Total, string Supply, int Min, int Max);

public enum MinMaxOptions
{
    Weekdays,
    Weekends,
    Events
}