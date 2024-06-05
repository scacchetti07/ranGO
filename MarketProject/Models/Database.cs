using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace MarketProject.Models;

public class Database
{
    public List<Product> Products { get; private set; } = new();
    
    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        Products.Remove(product);
    }

    public void Serialize()
    {
        string json = JsonConvert.SerializeObject(Products);
        using StreamWriter sw = new StreamWriter(File.Open(@".\teste.json",FileMode.Create));
        sw.WriteLine(json);
    }

    public void Deserialize()
    {
        string path = @".\teste.json";
        if (!File.Exists(path))
            return;
        using StreamReader sr = new StreamReader(path);
        string json = sr.ReadToEnd();
        Products = JsonConvert.DeserializeObject<List<Product>>(json)!;
    }
}