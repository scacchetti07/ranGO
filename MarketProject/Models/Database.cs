using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MarketProject.Models;

public static class Database
{
    public static void ConnectDataBase()
    {
        const string connectionUri = "mongodb+srv://admin:admin@rango.epqgj.mongodb.net/?retryWrites=true&w=majority&appName=ranGO";
        
        var settings = MongoClientSettings.FromConnectionString(connectionUri);
        // Set the ServerApi field of the settings object to set the version of the Stable API on the client
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        // Create a new client and connect to the server
        var client = new MongoClient(settings);
        // Send a ping to confirm a successful connection
        try {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }  
    }
    // GetSupplyProductBy(Supply supply) {}

}