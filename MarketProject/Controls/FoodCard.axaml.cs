using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using DynamicData;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Controls;

public partial class FoodCard : UserControl
{
    public static readonly StyledProperty<string> FoodNameProperty =
        AvaloniaProperty.Register<FoodCard, string>(nameof(FoodName));

    public static readonly StyledProperty<string> FoodIngredientsProperty =
        AvaloniaProperty.Register<FoodCard, string>(nameof(FoodIngredients));

    public static readonly StyledProperty<double> FoodPriceProperty =
        AvaloniaProperty.Register<FoodCard, double>(nameof(FoodPrice));

    public static readonly StyledProperty<string> FoodPicturePathProperty =
        AvaloniaProperty.Register<FoodCard, string>(nameof(FoodPicturePath));

    public string FoodName
    {
        get => GetValue(FoodNameProperty);
        set => SetValue(FoodNameProperty, value);
    }

    public string FoodIngredients
    {
        get => GetValue(FoodIngredientsProperty);
        set => SetValue(FoodIngredientsProperty, value);
    }

    public double FoodPrice
    {
        get => GetValue(FoodPriceProperty);
        set => SetValue(FoodPriceProperty, value);
    }

    public string FoodPicturePath
    {
        get => GetValue(FoodPicturePathProperty);
        set => SetValue(FoodPicturePathProperty, value);
    }

    public FoodCard()
    {
        InitializeComponent();
        FoodNameProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
        FoodIngredientsProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
        FoodPriceProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
        FoodPicturePathProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
    }

    private void UpdateFoodCard()
    {
        FoodNameLabel.Content = FoodName;
        FoodIngredientsTextBlock.Text = FoodIngredients;
        FoodPriceTextBlock.Text = FoodPrice.ToString("f2", new CultureInfo("pt-BR")).Insert(0, "R$ ");

        try
        {
            var newImageBrush = FoodPicturePath is null
                ? new ImageBrush(
                    new Bitmap(AssetLoader.Open(new Uri("avares://MarketProject/Assets/DefaultFoodBackground.jpg"))))
                : new ImageBrush(new Bitmap(FoodPicturePath));

            newImageBrush.Stretch = Stretch.UniformToFill;
            FoodPictureImageBrush.Background = newImageBrush;
        }
        catch (FileNotFoundException)
        {
            FoodPictureImageBrush.Background = new ImageBrush(
                new Bitmap(AssetLoader.Open(new Uri("avares://MarketProject/Assets/DefaultFoodBackground.jpg"))))
            {
                Stretch = Stretch.UniformToFill
            };
        }
    }

    private async void EditFoodButton_OnClick(object sender, RoutedEventArgs e)
    {
        var food = await FoodMenuController.FindFoodMenuByNameAsync(FoodName);
        ManageFoodView manageFoodView = new ManageFoodView(food.Id)
        {
            Title = "Cadastro de Pratos",
        };

        manageFoodView.ShowDialog(
            (Window)Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!);
        var newFood = await manageFoodView.GetFood();
        FoodMenuController.EditFoodMenu(newFood);
    }

    private async void DeleteFoodButton_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedFood = await FoodMenuController.FindFoodMenuByNameAsync(FoodNameLabel.Content!.ToString())
            .ConfigureAwait(false);

        Dispatcher.UIThread.Post(async () =>
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Excluir prato do cardápio",
                ContentMessage =
                    $"Você realmente deseja excluir \"{selectedFood.FoodName}\" do cardápio em definitivo?",
                ButtonDefinitions = ButtonEnum.YesNo,
                Icon = Icon.Warning,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            var result = await msgBox.ShowAsync().ConfigureAwait(false);
            if (result == ButtonResult.No) return;

            var orders = Database.OrdersList.Where(o => o.FoodsOrder.Contains(selectedFood.Id));
            foreach (var ord in orders)
            {
                ord!.FoodsOrder.Remove(selectedFood.Id);
                OrderController.EditOrder(ord);  
            }
            FoodMenuController.DeleteFoodMenu(selectedFood);
            File.Delete(selectedFood.FoodPhotoPath);
            Database.FoodsMenuList.Remove(selectedFood);
        });
    }
}