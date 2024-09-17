using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using ReactiveUI;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace MarketProject.Models;

public class Database
{
    public Database()
    {
        _client.StartSession();
        Console.WriteLine("Sou o DataBase!");
    }
    
    
    public static ObservableCollection<Product> ProductsList { get; private set; } = new();

    private static MongoClient _client =
        new("mongodb+srv://luiscacchetti07:c2qb5VFjcVBA18PH@rango.3fwol.mongodb.net/");

    protected static IMongoDatabase GetDatabase(string dbName)
        => _client.GetDatabase(dbName);

    protected static IMongoCollection<BsonDocument> GetCollectionBson(string dbName, string dbCollection)
        => GetDatabase(dbName).GetCollection<BsonDocument>(dbCollection);

    public async void Deserialize()
    {
        var collectionProducts = GetDatabase("storage").GetCollection<Product>("products");

        var docs = await collectionProducts.Find(Builders<Product>.Filter.Empty).ToListAsync();
        //string jsonList = docsBson.ToJson();
        Console.WriteLine(docs.Count);
        foreach (var d in docs)
        {
            Console.WriteLine(d);
        }

        
        //var products = JsonConvert.DeserializeObject<List<Product>>(jsonList);
        ProductsList = new ObservableCollection<Product>(docs);
        Console.WriteLine("Lista de produtos atualizada!");
        
    }
    
    // GetSupplyProductBy(Supply supply) {}
}