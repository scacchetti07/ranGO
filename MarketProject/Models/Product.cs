#nullable enable
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace MarketProject.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; internal set; }
    public long Gtin { get; set; }
    public string Unit { get; set; } // Unidade de Medida
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; }
    public int Total { get; set; }
    public Range<int> Weekdays { get; set; } = new(0, 0); // Min Max
    public Range<int> Weekends { get; set; } = new(0, 0); // Min Max
    public Range<int> Events { get; set; } = new(0, 0); // Min Max
    
    public Product(long gtin, string prodName, decimal price, string unit, Range<int> weekdays, Range<int> weekends, Range<int> events, string? description = null, int prodTotal = 0)
    {
        Gtin = gtin;
        Description = description;
        Name = prodName;
        Price = price;
        Unit = unit;
        Total = prodTotal;
        Weekdays = weekdays;
        Weekends = weekends;
        Events = events;
    }

    public Product()
    { }

}