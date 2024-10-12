using System;
using System.Collections.Generic;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Media;
using MarketProject.Controls;

namespace MarketProject.Models;

public class Orders
{
    public Orders(int tableNumber, string waiterName, List<Foods> foods, OrderStatusEnum orderStatus)
    {
        Id = new Guid().ToString();
        FoodsOrder = foods;
        TableNumber = tableNumber;
        WaiterName = waiterName;
        OrderStatus = orderStatus;
    }
    
    public string Id { get; private set; }
    public int TableNumber { get; set; }
    public List<Foods> FoodsOrder { get; set; }
    public string WaiterName { get; set; }
    public OrderStatusEnum OrderStatus { get; set; }
    
}