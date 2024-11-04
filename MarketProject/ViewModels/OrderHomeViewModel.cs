using System;
using System.Collections.Generic;
using System.Linq;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;
namespace MarketProject.ViewModels;

public class OrderHomeViewModel : ViewModelBase
{
    public OrderStatusEnum OrderStatus;
    public OrderCards OrderToCard(Orders order)
    {
        List<string> FoodOrderNames = new();
        FoodOrderNames.AddRange(order.FoodsOrder.Select(foodId =>
            FoodMenuController.FindFoodMenu(foodId).Result.FoodName));

        OrderCards orderCards = new()
        {
            WaiterName = order.WaiterName,
            FoodOrderNames = String.Join(", ", FoodOrderNames.TakeLast(2)) + $"+ {FoodOrderNames.Count}",
            TableNumber = order.TableNumber,
            Id = order.Id,
            OrderStatus = order.OrderStatus
        };
        return orderCards;
    }

    public static FoodCard FoodToCard(Foods food)
    {
        List<string> nameOfIngredients = Database.ProductsList.Count != 0 ?
            food.ListOfIngredients.Select(id => StorageController.FindProduct(id).Name).ToList() : null;
        return new FoodCard
        {
            FoodName = food.FoodName,
            FoodIngredients = nameOfIngredients is not null
                ? string.Join(",", nameOfIngredients.Take(1))+ $"  + {nameOfIngredients.Count - 1}" : "",
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