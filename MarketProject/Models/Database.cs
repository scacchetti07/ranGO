using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using MarketProject.Models;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MarketProject.Models;

public class Database
{
    public static ObservableCollection<Product> ProductsList { get; } = new();

    private static MongoClient _client =
        new("mongodb+srv://luiscacchetti07:c2qb5VFjcVBA18PH@rango.3fwol.mongodb.net/");
    
    public Database()
    {
        _client.StartSession();
    }
    
    // GetSupplyProductBy(Supply supply) {}
    public static void AddProduct(Product product)
    {
        var collection = _client.GetDatabase("storage").GetCollection<BsonDocument>("products");
        var newProduct = new BsonDocument
        {
            {"_id", product.Id },
            {"gtin", product.GTIN},
            {"name", product.Name},
            {"price", product.Price},
            {"description", product.Description},
            {"unit", product.Unit},
            {"total", product.Total},
            {"minWeekdays", product.Weekday.Start.ToString()},
            {"maxWeekdays", product.Weekday.End.ToString()},
            {"minWeekends", product.Weekends.Start.ToString()},
            {"maxWeekends", product.Weekends.End.ToString()},
            {"minEvents", product.Events.Start.ToString()},
            {"maxEvents", product.Events.End.ToString()}
        };
       collection.InsertOne(newProduct);
       ProductsList.Add(product);
    }

}