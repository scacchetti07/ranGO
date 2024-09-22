using System;
using System.Collections;
using Avalonia;
using MarketProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using MarketProject.ViewModels;
using StorageCtrl = MarketProject.Controllers.StorageController;

namespace MarketProject.Views;

public partial class StorageView : UserControl
{ 
    public delegate void ProductChangedDelegate(Product product);
    public event ProductChangedDelegate ProductChanged;
    
    private Product _selectedProducts;
    
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

    private async void ProductsDataGrid_OnCellPointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        var productRow = ProductsDataGrid.SelectedItems.Cast<ProductDataGrid>();
        // foreach (var p in productRow)
        //     _selectedProducts = await StorageCtrl.FindProductAsync(p.Gtin).ConfigureAwait(false);
        if (_selectedProducts == null) return;
        Console.WriteLine(_selectedProducts.Gtin);
        
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
        await RegisProdView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!);
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        
    }
}