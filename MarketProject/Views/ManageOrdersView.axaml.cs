using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;

namespace MarketProject.Views;

public partial class ManageOrdersView : Window
{
    private TaskCompletionSource<Orders> _task = new();
    public ManageOrdersView()
    {
        InitializeComponent();
    }
    
    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    public async Task<Orders> GetOrder() => await _task.Task;
    private void Button2_OnClick(object sender, RoutedEventArgs e)
    {
        var foodList = new List<Foods>
        {
            new("Salada de Fruta",
                new List<Product>()
                {
                    StorageController.FindProductByNameAsync("Banana"),
                    StorageController.FindProductByNameAsync("Jabuticaba")
                }, "Salada de frutas"),
            
            new("Vitamina Proteica",
                new List<Product>()
                {
                    StorageController.FindProductByNameAsync("Whey Protein"),
                    StorageController.FindProductByNameAsync("Moranguinho")
                }, "Shake do Monstro")

        };
        var orderStatus = OrderStatusEnum.New;
        var newOrder = new Orders(1, "Pedro da Silva", foodList.Select(f => f.FoodName).ToList(), OrderStatusEnum.New);
        _task.SetResult(newOrder); // Sinaliza que o card foi adicionado.
    }
}