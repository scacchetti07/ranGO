using System.Collections.Generic;
using System.Threading.Tasks;
using MarketProject.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace MarketProject.Controllers;

public class StorageController : Database
{
    public static IMongoCollection<Product> Collection { get; } = GetCollection<Product>("storage", "products");
    public static async void AddProduct(Product product)
    {
        await Collection.InsertOneAsync(product);
        ProductsList.Add(product);
    }

    public static async void RemoveTotalProduct(Product product, int total)
    {
        var collection = GetDatabase("storage").GetCollection<Product>("product");
        var basicFilter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        var updateFilter = Builders<Product>.Update.Inc(p => p.Total, -total);

        await collection.UpdateOneAsync(basicFilter, updateFilter).ConfigureAwait(false);
    }

    public static async Task<List<Product>> FindProduct(int gtin = 0, string name = null)
    {
        var collection = GetDatabase("storage").GetCollection<Product>("products");
        
        FilterDefinition<Product> filter = null;
        if (gtin != 0)
            filter = Builders<Product>.Filter.Eq(p => p.Gtin, gtin);
        else if (name is not null)
            filter = Builders<Product>.Filter.Eq(p => p.Name, name);

        return await collection.Find(filter).ToListAsync();
    }
}