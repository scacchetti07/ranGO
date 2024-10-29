using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class FoodMenuView : UserControl
{
    private static readonly StyledProperty<ObservableCollection<Foods>> FoodsProperty =
        AvaloniaProperty.Register<FoodMenuView, ObservableCollection<Foods>>(nameof(FoodMenuController));
    public FoodMenuView()
    {
        InitializeComponent();
        UpdateFood();
        FoodsProperty.Changed.AddClassHandler<FoodMenuView>((_, _) => UpdateFood());
    }

    private async void UpdateFood()
    {
        FoodMenuCardsPanel.Children.Clear();
        var foodList = FoodMenuController.FindFoodMenu();
        if (foodList is null) return;
        foreach (Foods food in foodList)
        {
            var card = OrderHomeViewModel.FoodToCard(food);
            FoodMenuCardsPanel.Children.Add(card);
        }
    }

    // private async void UpdateFood(FoodTypesEnum? foodtype)
    // {
    //     if (foodtype is null) return;
    //
    //     var searchFood = await FoodMenuController.FindFoodMenu((FoodTypesEnum)foodtype);
    //     FoodMenuCardsPanel.Children.Clear();
    //     Dispatcher.UIThread.Post(() =>
    //     {
    //         foreach (Foods food in searchFood)
    //             FoodMenuCardsPanel.Children.Add(Vm.FoodToCard(food));
    //     });
    // }
    //
    // private void UpdateFood(IEnumerable<Foods> searchedList)
    // {
    //     FoodMenuCardsPanel.Children.Clear();
    //     Dispatcher.UIThread.Post(() =>
    //     {
    //         foreach (Foods food in searchedList)
    //             FoodMenuCardsPanel.Children.Add(Vm.FoodToCard(food));
    //     });
    // }

    private async void AddFoodButton_OnClick(object sender, RoutedEventArgs e)
    {
        ManageFoodView manageFoodView = new ManageFoodView { Title = "Cadastro de Pratos" };
        manageFoodView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent);
        var newFood = await manageFoodView.GetFood();
        FoodMenuController.AddNewFoodMenu(newFood);
    }
}