using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MarketProject.Controllers;
using MarketProject.Models;

namespace MarketProject.Controls;

public partial class FoodDashboardCards : UserControl
{
    public static readonly StyledProperty<Foods> CurrentFoodProperty =
        AvaloniaProperty.Register<FoodDashboardCards, Foods>(nameof(CurrentFood));
    public Foods CurrentFood
    {
        get => GetValue(CurrentFoodProperty);
        set => SetValue(CurrentFoodProperty, value);
    }
    public FoodDashboardCards()
    {
        InitializeComponent();
        CurrentFoodProperty.Changed.AddClassHandler<FoodDashboardCards>((_, _) => UpdateFoodCard());
    }

    private void UpdateFoodCard()
    {
        FoodNameTextBlock.Text = CurrentFood.FoodName;
        List<string> nameOfIngredients =
            CurrentFood.ListOfIngredients.Select(id => StorageController.FindProduct(id).Name).ToList();
        IngredientsListTextBlock.Text = string.Join(", ", nameOfIngredients);
        try
        {
            var newImageBrush = CurrentFood.FoodPhotoPath is null
                ? new ImageBrush(
                    new Bitmap(AssetLoader.Open(new Uri("avares://MarketProject/Assets/DefaultFoodBackground.jpg"))))
                : new ImageBrush(new Bitmap(CurrentFood.FoodPhotoPath));
            
            newImageBrush.Stretch = Stretch.UniformToFill;
            FoodImageBorder.Background = newImageBrush;
        }
        catch (FileNotFoundException)
        {
            FoodImageBorder.Background =  new ImageBrush(
                new Bitmap(AssetLoader.Open(new Uri("avares://MarketProject/Assets/DefaultFoodBackground.jpg"))))
            {
                Stretch = Stretch.UniformToFill
            };
        }
        
    }
}