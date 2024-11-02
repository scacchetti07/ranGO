using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using MarketProject.Controls;
using MarketProject.Models;
using MongoDB.Driver;

namespace MarketProject.Controllers;

public class OrderController : Database
{
    private static IMongoCollection<Orders> Collection { get; } = GetCollection<Orders>("storage", "orders");
    
    public static async void AddNewOrder(Orders order)
    {
        await Collection.InsertOneAsync(order);
        OrdersList.Add(order);
    }

    public static Orders FindOrders(string id)
    {
        var filter = Builders<Orders>.Filter.Eq(o => o.Id, id);
        return Collection.Find(filter).FirstOrDefault();
    }
    public static async Task<List<Orders>> FindOrders(OrderStatusEnum orderStatus)
    {
        var filter = Builders<Orders>.Filter.Eq(o => o.OrderStatus, orderStatus);
        return await Collection.Find(filter).ToListAsync();
    }

    public static async Task<List<Orders>> FindOrders()
        => await Collection.Find(Builders<Orders>.Filter.Empty).ToListAsync();
    public static async void EditOrder(Orders order)
    {
        var filter = Builders<Orders>.Filter.Eq(o => o.Id, order.Id); 
        await Collection.ReplaceOneAsync(filter, order);
        OrdersList.Replace(OrdersList.SingleOrDefault(o => o.Id == order.Id), order);
    }
    public static async void DeleteClosedOrders()
    {
        await Collection.DeleteManyAsync(o => o.OrderStatus == OrderStatusEnum.Closed).ConfigureAwait(false);
        OrdersList.Remove(OrdersList.Where(o => o.OrderStatus == OrderStatusEnum.Closed).ToList());
    }
}