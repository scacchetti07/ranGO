using System;
using System.Collections.Generic;
using System.Timers;

namespace MarketProject.Models;

public class Orders
{
    public Orders(int tableNumber, string waiterName, List<Foods> foods, OrderStatus orderStatus, double subTotal, Timer timer, Timer timerAttendment)
    {
        Id = new Guid().ToString();
        FoodsOrder = foods;
        TableNumber = tableNumber;
        WaiterName = waiterName;
        OrderStatus = orderStatus;
        SubTotal = subTotal;
        TimerAttendment = timerAttendment;
    }
    
    public string Id { get; private set; }
    public int TableNumber { get; set; }
    public List<Foods> FoodsOrder { get; set; }
    public string WaiterName { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public double SubTotal { get; set; }
    public Timer TimerAttendment { get; set; }
    
}

public enum OrderStatus
{
    New,
    Preparing,
    Closed
}