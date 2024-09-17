using MarketProject.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MarketProject.Controllers;

public class StorageController : Database
{
    public static IMongoCollection<BsonDocument> Collection { get; } = GetCollectionBson("storage", "products");
    public static void AddProduct(Product product)
    {
        var newProduct = new BsonDocument
        {
            {"Gtin", product.Gtin},
            {"Name", product.Name},
            {"Description", product.Description},
            {"Price", product.Price},
            {"Total", product.Total},
            {"Unit", product.Unit},
        };
        Collection.InsertOne(newProduct);
        
        ProductsList.Add(product);
    }

    public static async void RemoveProduct(Product product)
    {
        var collection = GetDatabase("storage").GetCollection<Product>("product");
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);

        await collection.DeleteOneAsync(filter);
    }
}