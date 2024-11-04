using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;
using MongoDB.Driver;

namespace MarketProject.ViewModels;

public class DashboardViewModel
{
    public Supply CurrentSupply { get; set; } = Database.SupplyList.FirstOrDefault(s => s.InDeliver);
    public List<Foods> MostRecentlyFood { get; set; } = Database.FoodsMenuList.TakeLast(3).ToList();
    
}