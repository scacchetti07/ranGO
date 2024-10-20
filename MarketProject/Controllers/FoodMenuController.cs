using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using DynamicData;
using MarketProject.Controls;
using MarketProject.Models;
using MongoDB.Driver;

namespace MarketProject.Controllers;

public class FoodMenuController : Database
{
    private static IMongoCollection<Foods> Collection { get; } = GetCollection<Foods>("storage", "foodMenu");
    
    public static async void AddNewFoodMenu(Foods food)
    {
        await Collection.InsertOneAsync(food);
        FoodsMenuList.Add(food);
    }

    public static async Task<List<Foods>> FindFoodMenu(FoodTypesEnum foodType)
    {
        var filter = Builders<Foods>.Filter.Eq(fm => fm.FoodTypes, foodType);
        return await Collection.Find(filter).ToListAsync();
    }

    public static async Task<List<Foods>> FindFoodMenu()
        => await Collection.Find(Builders<Foods>.Filter.Empty).ToListAsync();
    
    public static async void EditFoodMenu(Foods food)
    {
        var filter = Builders<Foods>.Filter.Eq(fm => fm.Id, food.Id); 
        await Collection.ReplaceOneAsync(filter, food);
        FoodsMenuList.Replace(FoodsMenuList.SingleOrDefault(fm => fm.Id == food.Id), food);
    }
    
    public static async void DeleteFoodMenu(Foods food)
    {
        var filter = Builders<Foods>.Filter.Eq(fm => fm.Id, food.Id);
        await Collection.DeleteOneAsync(filter);
        FoodsMenuList.Remove(food);
    }
}