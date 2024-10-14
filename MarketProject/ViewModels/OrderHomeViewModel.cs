using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MarketProject.Controls;
using MarketProject.Models;
using Microsoft.VisualBasic;
using MongoDB.Driver.Linq;

namespace MarketProject.ViewModels;

public class OrderHomeViewModel : ViewModelBase
{
    public OrderCards OrderToCard(Orders order)
    {
        OrderCards orderCards = new()
        {
            WaiterName = order.WaiterName,
            FoodOrderNames = String.Join(", ", order.FoodsOrder.TakeLast(2)),
            TableNumber = order.TableNumber,
            Id = String.Join("", order.Id.TakeLast(4)).Insert(0, "#"),
            OrderStatus = order.OrderStatus
        };
        return orderCards;
    }
}