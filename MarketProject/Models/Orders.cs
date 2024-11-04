using System;
using System.Collections.Generic;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Media;
using MarketProject.Controls;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MarketProject.Models;

public class Orders
{
    [BsonElement]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get;  set; }
    public int TableNumber { get; set; }
    public List<string> FoodsOrder { get; set; }
    public string WaiterName { get; set; }
    public string FoodDescription { get; set; }
    public OrderStatusEnum OrderStatus { get; set; }
    
    [JsonConstructor]
    public Orders(string id, int tableNumber, string waiterName, List<string> foods, string foodDescription, OrderStatusEnum orderStatus)
    {
        Id = id;
        FoodsOrder = foods;
        TableNumber = tableNumber;
        WaiterName = waiterName;
        FoodDescription = foodDescription;
        OrderStatus = orderStatus;
    }
    public Orders(int tableNumber, string waiterName, List<string> foods, string foodDescription, OrderStatusEnum orderStatus)
    {
        FoodsOrder = foods;
        TableNumber = tableNumber;
        WaiterName = waiterName;
        FoodDescription = foodDescription;
        OrderStatus = orderStatus;
    }

    
}