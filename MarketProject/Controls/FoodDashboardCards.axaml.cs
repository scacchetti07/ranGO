using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.Views;

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
        List<string> nameOfIngredients = Database.ProductsList.Count != 0 ?
            CurrentFood.ListOfIngredients.Select(id => StorageController.FindProduct(id).Name).ToList() : null;
        IngredientsListTextBlock.Text = nameOfIngredients is not null ? string.Join(", ", nameOfIngredients) : "Ingredientes não encontrados.";
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
                new Bitmap(AssetLoader.Open(new Uri("avares://MarketProject/Assets/DefaultFoodDashboard_bg.jpg"))))
            {
                Stretch = Stretch.UniformToFill
            };
        }
        
    }

    private void FoodCardEditButton_OnClick(object sender, RoutedEventArgs e)
    {
        ManageFoodView manageFoodView = new(CurrentFood.Id)
        {
            Title = "Editar Prato - ranGO!"
        };
        manageFoodView.TitleFoodManage.Text = "MODO VISUALIZAÇÃO";
        manageFoodView.TitleFoodManage.Foreground = Brush.Parse("#D87249");
        
        manageFoodView.CategoryComboBox.IsEnabled = false;
        manageFoodView.NameTextBox.IsEnabled = false;
        manageFoodView.PriceTextBox.IsEnabled = false;
        manageFoodView.DescriptionTextBox.IsEnabled = false;
        manageFoodView.TagContentStackPanel.IsEnabled = false;
        manageFoodView.ProductsAutoCompleteBox.IsEnabled = false;
        manageFoodView.AddButton.IsEnabled = false;
        manageFoodView.ClearButton.IsEnabled = false;
        manageFoodView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!.Parent!);
    }
}