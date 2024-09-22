using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using MarketProject.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Controllers;

public class StorageController : Database
{
    public static IMongoCollection<Product> Collection { get; } = GetCollection<Product>("storage", "products");
    public static async void AddProduct(Product product)
    {
        await Collection.InsertOneAsync(product).ConfigureAwait(false);
        ProductsList.Add(product);
    }

    public static async void RemoveTotalProduct(Product product, int total)
    {
        var collection = GetCollection<Product>("storage","products");
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        
        var update = Builders<Product>.Update.Inc(p => p.Total, -total);
        
        var result = await collection.UpdateOneAsync(filter, update).ConfigureAwait(false);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
            product.Total -= total;
    }

    public static Product FindProductAsync(long gtin = 0, string name = null)
    {
        var collection = GetCollection<Product>("storage","products");
        
        FilterDefinition<Product> filter = null;
        if (gtin != 0)
            filter = Builders<Product>.Filter.Eq(p => p.Gtin, gtin);
        else if (name != null)
            filter = Builders<Product>.Filter.Eq(p => p.Name, name);

        return collection.Find(filter).FirstOrDefault();
    }
}