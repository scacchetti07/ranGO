using System.Collections.Immutable;
using System.Linq;
using MarketProject.Controls;
using MarketProject.Models;

namespace MarketProject.ViewModels;

public class OrderHomeViewModel : ViewModelBase
{
    public OrderCards OrderToCard(Orders order)
        => new OrderCards()
        {
            WaiterName = order.WaiterName,
            FoodOrderNames = order.FoodsOrder.Select(f => f.FoodName),
            TableNumber = order.TableNumber.ToString("D2").Insert(0,"Mesa "),
            Id = order.Id,
            OrderStatus = order.OrderStatus
        };
}