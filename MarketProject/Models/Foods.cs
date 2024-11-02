using System;
using System.Collections.Generic;
using System.Dynamic;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MarketProject.Models;

public class Foods
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string FoodName { get; set; }
    public double FoodPrice { get; set; }
    public List<string> ListOfIngredients { get; set; }
    public string FoodDescription { get; set; }
    public FoodTypesEnum? FoodTypes { get; set; }
    public string FoodPhotoPath { get; set; }
    
    [JsonConstructor]
    public Foods(string id, string foodName, List<string> listOfIngredients, double foodPrice, FoodTypesEnum? foodTypes, string foodDescription = "", string foodPhotoPath = null)
    {
        Id = id;
        FoodName = foodName;
        FoodPhotoPath = foodPhotoPath;
        FoodPrice = foodPrice;
        FoodTypes = foodTypes;
        FoodDescription = foodDescription;
        ListOfIngredients = listOfIngredients;
    }
    public Foods(string foodName, List<string> listOfIngredients, double foodPrice, FoodTypesEnum? foodTypes, string foodDescription = "", string foodPhotoPath = null)
    {
        FoodName = foodName;
        FoodPhotoPath = foodPhotoPath;
        FoodPrice = foodPrice;
        FoodTypes = foodTypes;
        FoodDescription = foodDescription;
        ListOfIngredients = listOfIngredients;
    }
}

public enum FoodTypesEnum
{
    Appetizer, // Entrada
    Main, // Prato principal
    Salad,
    Pasta,
    Meat,
    Fish,
    Sandwich,
    Desserts, // Sobremesa
    Drink,
    Vegan,
    Kids
}