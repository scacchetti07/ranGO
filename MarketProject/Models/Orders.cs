using System;
using System.Collections.Generic;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Media;
using MarketProject.Controls;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MarketProject.Models;

public class Orders
{
    public Orders(int tableNumber, string waiterName, List<string> foods, OrderStatusEnum orderStatus)
    {
        FoodsOrder = foods;
        TableNumber = tableNumber;
        WaiterName = waiterName;
        OrderStatus = orderStatus;
    }
    
    [BsonElement]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }
    public int TableNumber { get; set; }
    public List<string> FoodsOrder { get; set; }
    public string WaiterName { get; set; }
    public OrderStatusEnum OrderStatus { get; set; }
    
}