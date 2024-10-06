using System;
using System.Collections;
using Avalonia;
using MarketProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;
using MarketProject.Controllers;
using MarketProject.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using ReactiveUI;
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
        
        // Resolução do Erro: Call Invalid Thread
        Database.ProductsList.CollectionChanged += (sender, _) =>
        {
            // Faz com que o código que atualiza o datagrid seja atualizado na UIThread.
            Dispatcher.UIThread.Post(() =>
            {
                ProductsDataGrid.ClearValue(DataGrid.ItemsSourceProperty);
                ProductsDataGrid.ItemsSource = (sender as ObservableCollection<Product>)!
                    .Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex));
            }, DispatcherPriority.Background);
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
        
        ProductsDataGrid.ClearValue(DataGrid.ItemsSourceProperty);
        ProductsDataGrid.ItemsSource = Database.ProductsList
            .Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex)); 
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
        var productRow = ProductsDataGrid.SelectedItems.Cast<ProductDataGrid>();
        foreach (var p in productRow)
            _selectedProducts = StorageCtrl.FindProduct(p.Gtin);
    }

    private async void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selectedProducts is null) return;
        
        ProdRegisterView editProduct = new()
        {
            Title = "Cadastro de Produtos",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        try
        {
            var supplyName = SupplyController.GetSupplyNameByProduct(_selectedProducts);
            
            editProduct.GtinTextBox.Text = _selectedProducts.Gtin.ToString();
            editProduct.NameTextBox.Text = _selectedProducts.Name;
            editProduct.DescriptionTextBox.Text = _selectedProducts.Description;
            editProduct.PriceTextBox.Text = _selectedProducts.Price.ToString();
            editProduct.UnitComboBox.SelectedValue = _selectedProducts.Unit;
            
            editProduct.SupplyAutoCompleteBox.Text = supplyName;
            editProduct.QuantityTextBox.Text = _selectedProducts.Total.ToString();
            
            editProduct.MinMaxView.MinTextBox.Text = _selectedProducts.Weekdays.Min.ToString();
            editProduct.MinMaxViewModel.WeekdaysMin = _selectedProducts.Weekdays.Min;
            editProduct.MinMaxView.MaxTextBox.Text = _selectedProducts.Weekdays.Max.ToString();
            editProduct.MinMaxViewModel.WeekdaysMax = _selectedProducts.Weekdays.Max;
            editProduct.MinMaxViewModel.EventsMin = _selectedProducts.Events.Min;
            editProduct.MinMaxViewModel.EventsMax = _selectedProducts.Events.Max;
            editProduct.MinMaxViewModel.WeekendsMin = _selectedProducts.Weekends.Min;
            editProduct.MinMaxViewModel.WeekendsMax = _selectedProducts.Weekends.Max;
            
            await editProduct.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!);
        }
        catch (NullReferenceException)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Produto não selecionado ou inválidao!",
                ContentMessage = "Tente selecionar o produto novamente.",
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = Icon.Warning,
                CanResize = false,
                ShowInCenter = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly
            });
            await msgBox.ShowAsync().ConfigureAwait(false);  
        }
        
    }

    private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selectedProducts is null) return;
        
        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Excluir produto do estoque",
            ContentMessage = $"Você realmente deseja excluir \"{_selectedProducts.Name}\" do estoque em definitivo?",
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
        
        StorageCtrl.DeleteProduct(_selectedProducts);
    }

    private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = SearchTextBox.Text;
        if (keyword.Length < 1)
        {
            ProductsDataGrid.ItemsSource = Database.ProductsList!
                .Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex));
            return;
        }
        
        var checkGtin = long.TryParse(keyword, out long gtin);
        IEnumerable<Product> searchedList;
        if (checkGtin)
            searchedList = Database.ProductsList.Where(p => p.Gtin.ToString().Contains($"{gtin}"));
        else
            searchedList = Database.ProductsList.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));
            
        ProductsDataGrid.ItemsSource = searchedList!.Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex));
    }
}