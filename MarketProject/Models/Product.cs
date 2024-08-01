#nullable enable
using System;
using System.Linq;
using Avalonia.Platform.Storage;

namespace MarketProject.Models;

public class Product
{
    public string Id { get; set; }
    public int GTIN { get; set; }
    public string? Unit { get; set; } // Unidade de Medida
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string ProdName { get; set; }
    public int ProdTotal { get; set; }
    public string Supply { get; set; }
    //public string ProdStatus { get; set; }
    public int[] ProdMin { get; set; } // Recebe o minimo dos: dias atuais, finais de semana e eventos
    public int[] ProdMax { get; set; } // Recebe o máximo dos: dias atuais, finais de semana e eventos
    
    public Range Weekday { get; set; }
    public Range Weekends { get; set; }
    public Range Events { get; set; }
    
    
    public Product(int gtin, string prodName, string supply, decimal price, string unit, Range weekday, Range weekends, Range events, string description = null, int prodTotal = 0)
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


}