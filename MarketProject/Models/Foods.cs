using System.Collections.Generic;
using System.Dynamic;
using Avalonia.Media;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MarketProject.Models;

public class Foods
{
    public Foods(string foodName, List<Product> listOfIngredients, string foodDescription = "", IImage foodPhoto = null)
    {
        Id = new ObjectId();
        FoodName = foodName;
        FoodPhoto = foodPhoto;
        FoodDescription = foodDescription;
        ListOfIngredients = listOfIngredients;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string FoodName { get; set; }
    public List<Product> ListOfIngredients { get; set; }
    public string FoodDescription { get; set; }
    public IImage FoodPhoto { get; set; }
    
}