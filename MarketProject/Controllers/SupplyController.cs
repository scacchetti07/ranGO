using MarketProject.Models;
using MongoDB.Driver;

namespace MarketProject.Controllers;

public class SupplyController : Database
{
    // GetSupplyProductBy(Supply supply) {}

    private static IMongoCollection<Supply> Collection { get; } = GetCollection<Supply>("storage", "supplys");
    public static async void AddNewSupply(Supply supply)
    {
        await Collection.InsertOneAsync(supply).ConfigureAwait(false);
        SupplyList.Add(supply);
    }
    
    public static Supply FindSupply(string cnpj = null, string name = null)
    {
        var collection = GetCollection<Supply>("storage","products");
        
        FilterDefinition<Supply> filter = null;
        if (cnpj != null)
            filter = Builders<Supply>.Filter.Eq(p => p.Cnpj, cnpj);
        else if (name != null)
            filter = Builders<Supply>.Filter.Eq(p => p.Name, name);

        return collection.Find(filter).FirstOrDefault();
    }
    public static async void DeleteSupply(Supply supply)
    {
        var filter = Builders<Supply>.Filter.Eq(p => p.Id, supply.Id);
        await Collection.DeleteOneAsync(filter).ConfigureAwait(false);
        SupplyList.Remove(supply);
    }
}