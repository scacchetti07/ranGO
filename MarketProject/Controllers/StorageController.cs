using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using MarketProject.Models;
using MarketProject.ViewModels;
using MongoDB.Driver;

namespace MarketProject.Controllers;

public class StorageController : Database
{
    private static IMongoCollection<Product> Collection { get; } = GetCollection<Product>("storage", "products");
    public static async void AddProduct(Product product,string supplyName)
    {
        await Collection.InsertOneAsync(product).ConfigureAwait(false);
        SupplyController.AddProductToSupply(product,supplyName);
        ProductsList.Add(product);
    }

    public static async void RemoveTotalProduct(Product product, int total)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        
        var update = Builders<Product>.Update.Inc(p => p.Total, -total);
        
        var result = await Collection.UpdateOneAsync(filter, update).ConfigureAwait(false);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
            product.Total -= total;
    }

    public static Product FindProductAsync(long gtin)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Gtin, gtin);

        return Collection.Find(filter).FirstOrDefault();
    }
    
    public static Product FindProductAsync(string id) 
        => Collection.Find(p => p.Id == id).FirstOrDefault();
    
    public static Product FindProductByNameAsync(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, name);

        return Collection.Find(filter).FirstOrDefault();
    }

    public static async void DeleteProduct(Product product)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        await Collection.DeleteOneAsync(filter);
        
        ProductsList.Remove(ProductsList.SingleOrDefault(p => p.Id == product.Id));
    }

    public static async void UpdateStorage(Product prod)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, prod.Id);
        var r = await Collection.ReplaceOneAsync(filter, prod);
        Console.WriteLine(r.ModifiedCount);
        
        //ProductsList.Replace(ProductsList.Where(p => p.Id == prod.Id).Single(), prod);
    }

    public static List<Product> FindProductsFromSupply(Supply supply) =>
        Collection.Find(p => supply.Products.Contains(p.Id)).ToList();
}