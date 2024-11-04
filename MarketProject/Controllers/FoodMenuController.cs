using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using DynamicData;
using MarketProject.Controls;
using MarketProject.Models;
using MongoDB.Bson;
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
        return await Collection.Find(filter).ToListAsync().ConfigureAwait(false);
    }

    public static List<Foods> FindFoodMenu()
        => Collection.Find(Builders<Foods>.Filter.Empty).ToList();
    public static async Task<Foods> FindFoodMenu(string id)
        => await Collection.Find(fm => fm.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
    public static async Task<Foods> FindFoodMenuByNameAsync(string name)
        => await Collection.Find(fm => fm.FoodName == name).FirstOrDefaultAsync().ConfigureAwait(false);

    public static Foods FindFoodMenuByName(string name)
        => Collection.Find(fm => fm.FoodName == name).FirstOrDefault();

    public static async Task<IEnumerable<Foods>> FindFoodsByOrders(IEnumerable<string> foods)
    {
        List<Foods> foodsByOrders = new();
        foreach (var f in foods)
            foodsByOrders.Add(await FindFoodMenu(f));
        return foodsByOrders;
    }
    
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