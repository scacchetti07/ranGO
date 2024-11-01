using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MarketProject.Controllers;
using MarketProject.Controls;
using MarketProject.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace MarketProject.Views;

public partial class ManageOrdersView : Window
{
    public async Task<Orders> GetOrder() => await _task.Task;

    private TaskCompletionSource<Orders> _task = new();
    public List<Foods> AutoCompleteSelectedFoodsList { get; } = [];

    public delegate void NewOrderAdded(Orders? orders);

    public event NewOrderAdded OrderAdded;

    public ManageOrdersView()
    {
        InitializeComponent();
    }

    private void AddNewOrder_OnClick(object sender, RoutedEventArgs e)
    {
        // AutoCompleteSelectedFoodsList.Select(f => f.Id).ToList() -> Adicionar dps no campo de newOrder quando tiver pratos para adicionar
        var newOrder = new Orders(int.Parse(TableNumberTextBox.Text!), WaiterNameTextBox.Text,
            ["Banana Split", "Sorvete de Creme"], OrderStatusEnum.New);
        _task.TrySetResult(newOrder);
        OrderAdded?.Invoke(newOrder);
        Close();
    }

    private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
    {
        List<string> textBoxes = GetTextBoxes();
        if (textBoxes.TrueForAll(txt => string.IsNullOrEmpty(txt)) && AutoCompleteSelectedFoodsList.Count == 0)
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

    private List<string> GetTextBoxes()
        =>
        [
            TableNumberTextBox.Text,
            WaiterNameTextBox.Text,
            FoodDescriptionTextBox.Text,
            FoodsAutoCompleteBox.Text
        ];

    private async void FoodsAutoCompleteBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;

        Foods food = await FoodMenuController.FindFoodMenuByNameAsync(FoodsAutoCompleteBox.Text);
        if (food is null || AutoCompleteSelectedFoodsList.Any(p => p.Id == food.Id)) return;

        AutoCompleteSelectedFoodsList.Add(food);
        var itemSource = FoodsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
        itemSource.Remove(food.FoodName);
        FoodsAutoCompleteBox.ItemsSource = itemSource;

        FoodsAutoCompleteBox.Text = "";
        TagContentStackPanel.Children.Add(GenereteAutoCompleteTag(food));
    }

    public Border GenereteAutoCompleteTag(Foods food)
    {
        Label label = new() { Content = food.FoodName };
        Image image = new();
        StackPanel stackPanel = new() { Children = { label, image } };

        var border = new Border
        {
            Child = stackPanel,
            Classes = { "AutoCompleteTag" }
        };
        border.PointerPressed += (_, _) =>
        {
            AutoCompleteSelectedFoodsList.Remove(food);
            TagContentStackPanel.Children.Remove(border);

            var itemSource = FoodsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
            itemSource.Add(food.FoodName);
            FoodsAutoCompleteBox.ItemsSource = itemSource;
        };
        return border;
    }
}