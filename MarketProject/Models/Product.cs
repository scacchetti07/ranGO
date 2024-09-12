#nullable enable
using System;
using System.Linq;
using Avalonia.Platform.Storage;

namespace MarketProject.Models;

public class Product
{
    public Product(int gtin, string prodName, Supply supply, decimal price, string unit, Range weekday, Range weekends, Range events, string? description = null, int prodTotal = 0, string id = null)
    {
        Id = Guid.NewGuid().ToString(); // Gera um Id único
        GTIN = gtin;
        Description = description;
        ProdName = prodName;
        Price = price;
        Unit = unit;
        ProdTotal = prodTotal;
        Supply = supply;
        Weekday = weekday;
        Weekends = weekends;
        Events = events;
    }

    public Product()
    { }
    
    public string Id { get; set; }
    public int GTIN { get; set; }
    public string Unit { get; set; } // Unidade de Medida
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string ProdName { get; set; }
    public int ProdTotal { get; set; }
    public Supply Supply { get; set; }
    //public string ProdStatus { get; set; }
    public Range Weekday { get; set; } // Min Max
    public Range Weekends { get; set; } // Min Max
    public Range Events { get; set; } // Min Max
    
    public Index Min { get; set; }
    public Index Max { get; set; }
    

}