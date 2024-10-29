using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using MarketProject.Controllers;
using MarketProject.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Views;

public partial class ManageFoodView : Window
{
    public List<Product> AutoCompleteSelectedProducts { get; } = [];
    private ImageBrush _foodImage { get; set; }

    public delegate void FoodMenuAddedDelegate(Foods? foods);

    public event FoodMenuAddedDelegate FoodMenuAdded;
    
    private TaskCompletionSource<Foods> _task = new();

    public ManageFoodView()
    {
        InitializeComponent();
        
        ProductsAutoCompleteBox.AddHandler(KeyDownEvent, (sender, e) =>
        {
            if (sender is not AutoCompleteBox autoComplete)
                return;
            if (e.Key != Avalonia.Input.Key.Tab || autoComplete.Text is null || autoComplete.Text.Trim() == "")
                return;

            string item = autoComplete.ItemsSource!
                .Cast<string>()
                .FirstOrDefault(item =>
                    autoComplete.TextFilter?.Invoke(autoComplete.Text, item) ?? true);
            if (item is null)
                return;

            autoComplete.Text = item;
            autoComplete.CaretIndex = autoComplete.Text.Length;
            e.Handled = true;
        }, RoutingStrategies.Tunnel);

        ProductsAutoCompleteBox.ItemsSource = Database.ProductsList.Select(p => p.Name);
    }

    private List<string> GetTextBox()
    {
        return new()
        {
            NameTextBox.Text,
            DescriptionTextBox.Text,
            PriceTextBox.Text,
        };
    }

    public async Task<Foods> GetFood() => await _task.Task.ConfigureAwait(false);
    private void ProductsAutoCompleteBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Avalonia.Input.Key.Enter) return;

        Product prod = StorageController.FindProductByNameAsync(ProductsAutoCompleteBox.Text);
        if (prod is null || AutoCompleteSelectedProducts.Any(p => p.Id == prod.Id)) return;

        AutoCompleteSelectedProducts.Add(prod);
        var itemSource = ProductsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
        itemSource.Remove(prod.Name);
        ProductsAutoCompleteBox.ItemsSource = itemSource;

        ProductsAutoCompleteBox.Text = "";
        TagContentStackPanel.Children.Add(GenereteAutoCompleteTag(prod));
    }

    public Border GenereteAutoCompleteTag(Product product)
    {
        Label label = new() { Content = product.Name };
        Image image = new();
        StackPanel stackPanel = new() { Children = { label, image } };

        var border = new Border
        {
            Child = stackPanel,
            Classes = { "AutoCompleteTag" }
        };
        border.PointerPressed += (_, _) =>
        {
            AutoCompleteSelectedProducts.Remove(product);
            TagContentStackPanel.Children.Remove(border);

            var itemSource = ProductsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
            itemSource.Add(product.Name);
            ProductsAutoCompleteBox.ItemsSource = itemSource;
        };
        return border;
    }

    private void ClearTextBoxButton_OnClick(object sender, RoutedEventArgs e)
    {
        NameTextBox.Text = "";
        DescriptionTextBox.Text = "";
        PriceTextBox.Text = null;
        CategoryComboBox.SelectedIndex = 0;
        TextContentStackPanel.IsVisible = true;

        AutoCompleteSelectedProducts.Clear();
        TagContentStackPanel.Children.Clear();
        ProductsAutoCompleteBox.ItemsSource = Database.ProductsList.Select(p => p.Name);
    }

    private async void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        double foodPrice = Convert.ToDouble(PriceTextBox.Text.Replace("_", ""));
        List<string> textBoxes = GetTextBox();
        if (textBoxes.Any(string.IsNullOrEmpty)) return;
        if (!AutoCompleteSelectedProducts.Any()) return;

        GetFoodType(((CategoryComboBox.SelectedItem as ComboBoxItem)!).Content!.ToString(),
            out FoodTypesEnum? foodType);
        Foods newFood = new(NameTextBox.Text, AutoCompleteSelectedProducts.Select(p => p.Id).ToList(), foodPrice,
            foodType, DescriptionTextBox.Text);

        var oldFood = await FoodMenuController.FindFoodMenu(newFood.Id).ConfigureAwait(false);
        if (oldFood is not null)
        {
            newFood.Id = oldFood.Id;
            FoodMenuAdded?.Invoke(newFood);
            _task.SetResult(newFood);
            return;
        }
        
        FoodMenuAdded?.Invoke(newFood);
        _task.SetResult(newFood);
        Dispatcher.UIThread.Post(() =>
        {
            AutoCompleteSelectedProducts.Clear();
            TagContentStackPanel.Children.Clear(); 
        });
        
    }

    private async void AddImageProduct_OnClick(object sender, RoutedEventArgs e)
    {
        FilePickerOpenOptions fileoption = new()
        {
            AllowMultiple = false,
            Title = "Selecione a foto do produto",
            FileTypeFilter = new List<FilePickerFileType>
            {
                FilePickerFileTypes.ImagePng, FilePickerFileTypes.ImageJpg
            }
        };
        var result = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(fileoption);
        foreach (var r in result)
            _foodImage = new ImageBrush(new Bitmap(r.Path.LocalPath));
        ProductImage.Background = _foodImage;
        TextContentStackPanel.IsVisible = false;
    }

    private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
    {
        List<string> textBoxes = GetTextBox();
        if (textBoxes.TrueForAll(txt => string.IsNullOrEmpty(txt)) && AutoCompleteSelectedProducts.Count == 0)
        {
            Close();
            return;
        }

        Dispatcher.UIThread.Post(async () =>
        {
            var checkMsgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Dados ainda digitados.",
                ContentMessage =
                    "Ainda existem dados escritos nos campos de cadastro,\nQuer realmente sair do cadastro?",
                ButtonDefinitions = ButtonEnum.YesNo,
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            var result = await checkMsgBox.ShowAsync();
            if (result == ButtonResult.Yes) Close();
        }, DispatcherPriority.Background);
    }

    private void GetFoodType(string item, out FoodTypesEnum? foodType)
    {
        foodType = item switch
        {
            "Entrada" => FoodTypesEnum.Appetizer,
            "Principal" => FoodTypesEnum.Main,
            "Kids" => FoodTypesEnum.Kids,
            "Vegano" => FoodTypesEnum.Vegan,
            "Salada" => FoodTypesEnum.Salad,
            "Massa" => FoodTypesEnum.Pasta,
            "Peixes" => FoodTypesEnum.Fish,
            "Sanduíche" => FoodTypesEnum.Sandwich,
            "Carnes" => FoodTypesEnum.Meat,
            "Hambúrgueres" => FoodTypesEnum.Meat,
            "Sobremesas" => FoodTypesEnum.Desserts,
            "Bebidas" => FoodTypesEnum.Drink,
            _ => null
        };
    }
}