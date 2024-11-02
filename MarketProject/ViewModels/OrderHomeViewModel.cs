using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;
using MarketProject.Views;
using Microsoft.VisualBasic;
using MongoDB.Driver.Linq;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class OrderHomeViewModel : ViewModelBase
{
    public OrderCards OrderToCard(Orders order)
    {
        List<string> FoodOrderNames = new();
        FoodOrderNames.AddRange(order.FoodsOrder.Select(foodId => FoodMenuController.FindFoodMenu(foodId).Result.FoodName));
        
        OrderCards orderCards = new()
        {
            WaiterName = order.WaiterName,
            FoodOrderNames = String.Join(", ", FoodOrderNames.TakeLast(2)),
            TableNumber = order.TableNumber,
            Id = String.Join("", order.Id.TakeLast(4)).Insert(0, "#"),
            OrderStatus = order.OrderStatus
        };
        return orderCards;
    }
    
    public static FoodCard FoodToCard(Foods food)
    {
        List<string> nameOfIngredients = food.ListOfIngredients.Select(id => StorageController.FindProduct(id).Name).ToList();
        return new FoodCard
        {
            FoodName = food.FoodName,
            FoodIngredients = string.Join(',', nameOfIngredients.Take(1)),
            FoodPrice = food.FoodPrice,
            FoodPicturePath = food.FoodPhotoPath
        };
    }
    
    private string? _foodName;
    public string? FoodName
    {
        get => _foodName;
        set
        {
            _foodName = value;
            ClearErrors(nameof(FoodName));
            if (FoodName?.Trim() != "" && StorageController.FindProductByNameAsync(FoodName) is null)
                AddError(nameof(FoodName), "Produto digitado não no estoque!");
            else if (!string.IsNullOrEmpty(FoodName))
                AddError(nameof(FoodName), "Produto não foi adicionado. Clique 'Enter'");
            else
                RemoveError(nameof(FoodName));
        }
    }
}