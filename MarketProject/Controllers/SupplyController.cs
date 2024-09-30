using System;
using System.Threading.Tasks;
using MarketProject.Models;
using MongoDB.Driver;

namespace MarketProject.Controllers;

public class SupplyController : Database
{
    private static IMongoCollection<Supply> Collection { get; } = GetCollection<Supply>("storage", "supplys");
    public static async void AddNewSupply(Supply supply)
    {
        await Collection.InsertOneAsync(supply).ConfigureAwait(false);
        SupplyList.Add(supply);
    }

    public static Supply FindSupply(string cnpj) => Collection.Find(s => s.Cnpj == cnpj).FirstOrDefault();
    public static Supply FindSupplyByName(string name) => Collection.Find(s => s.Name == name).FirstOrDefault(); 
    
    
    public static async void DeleteSupply(Supply supply)
    {
        var filter = Builders<Supply>.Filter.Eq(p => p.Id, supply.Id);
        await Collection.DeleteOneAsync(filter).ConfigureAwait(false);
        SupplyList.Remove(supply);
    }

    public static string GetSupplyNameProductBy(Product product)
    {
        
        //var filter = Builders<Supply>.Filter.In(s => s.Products.Find(p => p.Id == product.Id));
        var supply = Collection.Find(s => s.Products.Contains(product.Id)).FirstOrDefault();
        
        // supply?.Name != null ? supply.Name : "Fornecedor Teste"; 
        return supply?.Name ?? "Fornecedor Teste";


    }

    public static void AddProductToSupply(Product product, string supplyName)
    {
        Supply supply = Collection.Find(s => s.Name == supplyName).FirstOrDefault();
        if (supply is null) return;
        
        supply.Products.Add(product.Id);
        
        Collection.ReplaceOne(s => s.Id == supply.Id, supply);
        
        // De uma olhada nisso => Collection.FindOneAndUpdate()
    }
}