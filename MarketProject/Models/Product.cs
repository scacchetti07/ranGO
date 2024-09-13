#nullable enable
using System;

namespace MarketProject.Models;

public class Product
{
    public Product(int gtin, string prodName, decimal price, string unit, Range weekday, Range weekends, Range events, string? description = null, int prodTotal = 0, string id = null)
    {
        Id = Guid.NewGuid().ToString(); // Gera um Id único
        GTIN = gtin;
        Description = description;
        Name = prodName;
        Price = price;
        Unit = unit;
        Total = prodTotal;
        Weekday = weekday;
        Weekends = weekends;
        Events = events;
    }

    public Product()
    { }
    
    public string Id { get; private set; }
    public int GTIN { get; set; }
    public string Unit { get; set; } // Unidade de Medida
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; }
    public int Total { get; set; }
    public Supply Supply { get; set; }
    public Range Weekday { get; set; } // Min Max
    public Range Weekends { get; set; } // Min Max
    public Range Events { get; set; } // Min Max
    
    public Index Min { get; set; }
    public Index Max { get; set; }
    

}