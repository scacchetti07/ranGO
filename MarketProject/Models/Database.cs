using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MarketProject.Models;

public class Database
{
    public Database()
    {
        _client.StartSession();
        Console.WriteLine("Conexão Estabelecida!");
    }

    private static ObservableCollection<Product> _productsList;

    public static ObservableCollection<Product> ProductsList { get; private set; } = new();

    private static MongoClient _client =
        new("mongodb+srv://luiscacchetti07:c2qb5VFjcVBA18PH@rango.3fwol.mongodb.net/");

    protected static IMongoDatabase GetDatabase(string dbName)
        => _client.GetDatabase(dbName);

    protected static IMongoCollection<BsonDocument> GetCollectionBson(string dbName, string dbCollection)
        => GetDatabase(dbName).GetCollection<BsonDocument>(dbCollection);
    protected static IMongoCollection<T> GetCollection<T>(string dbName, string dbCollection)
        => GetDatabase(dbName).GetCollection<T>(dbCollection);

    public async void StartStorage()
    {
        var collectionProducts = GetDatabase("storage").GetCollection<Product>("products");
        var docs = await collectionProducts.Find(Builders<Product>.Filter.Empty).ToListAsync();
        
        ProductsList = new ObservableCollection<Product>(docs);
        Console.WriteLine("Lista de produtos Iniciada!");
    }
    
    // GetSupplyProductBy(Supply supply) {}
}