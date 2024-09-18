#nullable enable
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MarketProject.Models;

public class Product
{
    public Product(long gtin, string prodName, decimal price, string unit, Range weekday, Range weekends, Range events, string? description = null, int prodTotal = 0)
    {
        Gtin = gtin;
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
    
    [BsonId]
    public ObjectId Id { get; set; }
    public long Gtin { get; set; }
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