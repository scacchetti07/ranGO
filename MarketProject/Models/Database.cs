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
    public static ObservableCollection<Product> ProductsList { get; private set; } = new();
    public static ObservableCollection<Supply> SupplyList { get; private set; } = new();
    public static ObservableCollection<Orders> OrdersList { get; private set; } = new();
    public static ObservableCollection<Foods> FoodsMenuList { get; private set; } = new();

    private static MongoClient _client =
        new("mongodb+srv://luiscacchetti07:c2qb5VFjcVBA18PH@rango.3fwol.mongodb.net/");

    protected static IMongoDatabase GetDatabase(string dbName)
        => _client.GetDatabase(dbName);
    protected static IMongoCollection<T> GetCollection<T>(string dbName, string dbCollection)
        => GetDatabase(dbName).GetCollection<T>(dbCollection);
    public Database()
    {
        _client.StartSession();
        Console.WriteLine("Conexão Estabelecida!");
    }
    public async void StartStorage()
    {
        var collectionProducts = GetDatabase("storage").GetCollection<Product>("products");
        var prodDocs = await collectionProducts.Find(Builders<Product>.Filter.Empty).ToListAsync();
        
        var collectionSupply = GetDatabase("storage").GetCollection<Supply>("supplys");
        var supplyDocs = await collectionSupply.Find(Builders<Supply>.Filter.Empty).ToListAsync();
        
        var collectionOrder = GetDatabase("storage").GetCollection<Orders>("orders");
        var orderDocs = await collectionOrder.Find(Builders<Orders>.Filter.Empty).ToListAsync();
        
        var collectionFoodMenu = GetDatabase("storage").GetCollection<Foods>("foodMenu");
        var foodsMenuDocs = await collectionFoodMenu.Find(Builders<Foods>.Filter.Empty).ToListAsync();
        
        ProductsList = new ObservableCollection<Product>(prodDocs);
        SupplyList = new ObservableCollection<Supply>(supplyDocs);
        OrdersList = new ObservableCollection<Orders>(orderDocs);
        FoodsMenuList = new ObservableCollection<Foods>(foodsMenuDocs);
        Console.WriteLine("Lista de produtos e fornecedores Iniciada!");
        Console.WriteLine("Listas de Pedidos e Cardápio iniciadas!");
    }
}