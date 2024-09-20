using System;
using Avalonia;
using MarketProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using MarketProject.ViewModels;
using ctrl = MarketProject.Controllers;

namespace MarketProject.Views;

public partial class StorageView : UserControl
{
    // Definindo uma propriedade para o avalonia do tipo produto
    public static readonly StyledProperty<List<Product>> ProductsProperty =
        AvaloniaProperty.Register<StorageView, List<Product>>(nameof(Database));
    
    public delegate void ProductChangedDelegate(Product product);
    public event ProductChangedDelegate ProductChanged;
    
    private IEnumerable<Product> _selectedProducts;
    
    private StorageViewModel _vm => DataContext as StorageViewModel;
    
    public StorageView()
    {
        InitializeComponent();
        ProductsDataGrid.ItemsSource = Database.ProductsList.Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex));
        Database.ProductsList.CollectionChanged += (sender, _) =>
        {
            ProductsDataGrid.ItemsSource = new List<Product>();
            ProductsDataGrid.ItemsSource = (sender as ObservableCollection<Product>)!
                .Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex)); 
        };
    }
    
    private async void RegisterProductButton(object sender, RoutedEventArgs e)
    {
        // fazer RegisProdView retornar um produto.
        ProdRegisterView RegisProdView = new()
        {
            Title = "Cadastro de Produtos",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
       await RegisProdView.ShowDialog((Window)this.Parent!.Parent!.Parent!.Parent!.Parent!);
       
       // Fazer evento acionar que ocorreu adição dos produtos.
       ProductChanged?.Invoke(new Product());
    }

    private void ChangeMinMaxTable_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ProductsDataGrid == null) return;
        PropertyChanged += (_, _) =>
        {
            ProductsDataGrid.ItemsSource = Database.ProductsList
                .Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex)); 
        };

    }

    private async void RemoveProductButton(object sender, RoutedEventArgs e)
    {
        RemoveProductView removeProductView = new()
        {
            Title = "Remover Produtos",
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
        };
        await removeProductView.ShowDialog((Window)this.Parent!.Parent!.Parent!.Parent!.Parent!).ConfigureAwait(false);
    }

    private void ProductsDataGrid_OnCellPointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        _selectedProducts = ProductsDataGrid.SelectedItems.Cast<Product>();
        ButtonOptions.IsVisible = e.Row.IsSelected;
    }

    private async void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
        ProdRegisterView RegisProdView = new()
        {
            Title = "Cadastro de Produtos",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        foreach (var p in _selectedProducts)
        {
            RegisProdView.NameTextBox.Text = p.Name;
            RegisProdView.GtinTextBox.Text = p.Gtin.ToString();
            RegisProdView.DescriptionTextBox.Text = p.Description;
            RegisProdView.QuantityTextBox.Text = p.Total.ToString();
            RegisProdView.UnitComboBox.SelectedValue = p.Unit;
            RegisProdView.PriceTextBox.Mask = p.Price.ToString();
            RegisProdView.MinMaxViewModel.WeekdaysMin = p.Weekday.Min;
            RegisProdView.MinMaxViewModel.WeekdaysMax = p.Weekday.Max;
            RegisProdView.MinMaxViewModel.WeekendsMin = p.Weekends.Min;
            RegisProdView.MinMaxViewModel.WeekendsMax = p.Weekends.Max;
            RegisProdView.MinMaxViewModel.EventsMax = p.Events.Max;
            RegisProdView.MinMaxViewModel.EventsMin = p.Events.Min;
        }
        await RegisProdView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!);
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        
    }
}