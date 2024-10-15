using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using MarketProject.Models;
using MarketProject.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MarketProject.Controllers;

public class StorageController : Database
{
    private static IMongoCollection<Product> Collection { get; } = GetCollection<Product>("storage", "products");
    public static async void AddProduct(Product product, string supplyName)
    {
        if (ProductsList.Select(p => p.Name == product.Name).FirstOrDefault()) return;
        
        await Collection.InsertOneAsync(product).ConfigureAwait(false);
        SupplyController.AddProductToSupply(product,supplyName);
        ProductsList.Add(product);
    }

    public static async void RemoveTotalProduct(Product product, int total)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        var update = Builders<Product>.Update.Inc(p => p.Total, -total);
        
        var result = await Collection.UpdateOneAsync(filter, update).ConfigureAwait(false);

        var oldProduct = ProductsList.SingleOrDefault(p => p.Id == product.Id);
        if (!result.IsAcknowledged || result.ModifiedCount <= 0) return;
        
        product.Total -= total;
        ProductsList.Replace(oldProduct, product);
    }

    public static Product FindProduct(long gtin)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Gtin, gtin);

        return Collection.Find(filter).FirstOrDefault();
    }
    
    public static Product FindProduct(string id) 
        => Collection.Find(p => p.Id == id).FirstOrDefault();
    
    public static Product FindProductByNameAsync(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, name);

        return Collection.Find(filter).FirstOrDefault();
    }

    public static async void DeleteProduct(Product product)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
        await Collection.DeleteOneAsync(filter).ConfigureAwait(false);
        
        ProductsList.Remove(ProductsList.SingleOrDefault(p => p.Id == product.Id));
    }

    public static async void UpdateStorage(Product prod, string supplyName)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, prod.Id);
        await Collection.ReplaceOneAsync(filter, prod).ConfigureAwait(false);
        SupplyController.ReplaceProductToSupply(prod, supplyName);
        ProductsList.Replace(ProductsList.SingleOrDefault(p => p.Id == prod.Id), prod);
    }

    public static List<Product> FindProductsFromSupply(Supply supply) =>
        Collection.Find(p => supply.Products.Contains(p.Id)).ToList();
}