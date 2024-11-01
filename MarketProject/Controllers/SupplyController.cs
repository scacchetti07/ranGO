using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using MarketProject.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Svg;

namespace MarketProject.Controllers;

public class SupplyController : Database
{
    private static IMongoCollection<Supply> Collection { get; } = GetCollection<Supply>("storage", "supplys");
    public static async void AddNewSupply(Supply supply)
    {
        await Collection.InsertOneAsync(supply).ConfigureAwait(false);
        SupplyList.Add(supply);
    }

    public static Supply FindSupplyByCnpj(string cnpj) => Collection.Find(s => s.Cnpj == cnpj).FirstOrDefault();
    public static Supply FindSupplyByName(string name) => Collection.Find(s => s.Name == name).FirstOrDefault();
    public static Supply FindSupply(string id) => Collection.Find(s => s.Id == id).FirstOrDefault();
    public static List<Supply> FindSupply() => Collection.Find(FilterDefinition<Supply>.Empty).ToList();
    
    public static async void DeleteSupply(Supply supply)
    {
        var filter = Builders<Supply>.Filter.Eq(p => p.Id, supply.Id);
        await Collection.DeleteOneAsync(filter).ConfigureAwait(false);
        
        SupplyList.Remove(SupplyList.SingleOrDefault(s => s.Id == supply.Id));
    }

    public static async void UpdateSupply(Supply newSupply)
    {
        var filter = Builders<Supply>.Filter.Eq(s => s.Id, newSupply.Id);
        await Collection.ReplaceOneAsync(filter, newSupply);
        SupplyList.Replace(SupplyList.SingleOrDefault(s => s.Id == newSupply.Id), newSupply);
    }
    
    public static string GetSupplyNameByProduct(Product product)
    {
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

    public static void ReplaceProductToSupply(Product product, string supplyName)
    {
        var currentSupply = Collection.Find(s => s.Products.Contains(product.Id)).FirstOrDefault();
        currentSupply?.Products.Remove(product.Id);

        var newSupply = Collection.Find(s => s.Name == supplyName).FirstOrDefault();
        if (newSupply is null) return;
        newSupply.Products.Add(product.Id);

        Collection.ReplaceOne(s => s.Id == newSupply.Id, newSupply);
        if (currentSupply is not null)
            Collection.ReplaceOne(s => s.Id == currentSupply.Id, currentSupply);
    }
}