using System;
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

    public DashboardViewModel()
    {
        Database.SupplyList.CollectionChanged += (sender, _) =>
        {
            var newList = sender as ObservableCollection<Supply>;
            CurrentSupply = newList!.FirstOrDefault(s => s.InDeliver);
            Console.Write(CurrentSupply.Name + ": ");
            Console.WriteLine(CurrentSupply.InDeliver);
        };
    }
        
}