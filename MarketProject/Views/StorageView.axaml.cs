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
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
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
        Database.SupplyList.CollectionChanged += (_, _) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                ProductsDataGrid.ClearValue(DataGrid.ItemsSourceProperty);
                ProductsDataGrid.ItemsSource = Database.ProductsList
                    .Select(p => StorageViewModel.ProductToDataGrid(p, (MinMaxOptions)SchedComboBox.SelectedIndex));
            }, DispatcherPriority.Background);
        };
    }
    
    private async void RegisterProductButton(object sender, RoutedEventArgs e)
    {
        ProductAddView RegisProdView = new()
        {
            Title = "Cadastro de Produtos",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        RegisProdView.ProductAdded += (product) =>
        {
            if (product is null)
            {
                DeletePopup.IsOpen = true;
                IconDeleteProd.Path = "/Assets/Icons/SVG/IconInfo.svg";
                DeleteProdLabel.Content = "Data Inválida";
                ContentDeleteTextBlock.Text = $"A data inserida é inferior a data atual '{DateTime.Now}'!";
                return;
            }
            AddPopup.IsOpen = true;
            AddProdLabel.Content = "Novo Produto Adicionado!";
            ContentAddTextBlock.Text = $"Produto '{product.Name}' foi adicionado com sucesso!";
        };
        await RegisProdView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!);
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
        removeProductView.ProductDeleted += (product) =>
        {
            DeletePopup.IsOpen = true;
            IconDeleteProd.Path = "/Assets/Icons/SVG/IconDeleteTotal.svg";
            DeleteProdLabel.Content = "Total do produto quitado!";
            ContentDeleteTextBlock.Text = $"O total do '{product.Name}' foi atualizado no sistema.";
        };
        await removeProductView.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!).ConfigureAwait(false);
    }

    private async void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
        var product = ProductsDataGrid.SelectedItems.Cast<ProductDataGrid>().FirstOrDefault();
        if (product is null) return;
        var selectedProducts = StorageCtrl.FindProduct(product.Gtin);
        
        ProductAddView editProduct = new()
        {
            Title = "Cadastro de Produtos",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        editProduct.AddButton.Content = "Editar";
        
        var supplyName = SupplyController.GetSupplyNameByProduct(selectedProducts);
        
        editProduct.GtinTextBox.Text = selectedProducts.Gtin.ToString();
        editProduct.GtinTextBox.IsEnabled = false;
        
        editProduct.NameTextBox.Text = selectedProducts.Name;
        editProduct.DescriptionTextBox.Text = selectedProducts.Description;
        editProduct.ValidityDatePicker.SelectedDate = selectedProducts.Validity;
        
        editProduct.PriceTextBox.Text = selectedProducts.Price.ToString("f2").PadLeft(6, '_');
        var item = editProduct.UnitComboBox.Items.SingleOrDefault(u => (u as ComboBoxItem).Content.ToString() == selectedProducts.Unit);
        editProduct.UnitComboBox.SelectedIndex = editProduct.UnitComboBox.Items.IndexOf(item);
        
        editProduct.SupplyAutoCompleteBox.Text = supplyName;
        editProduct.QuantityTextBox.Text = selectedProducts.Total.ToString();
        
        editProduct.MinMaxView.MinTextBox.Text = selectedProducts.Weekdays.Min.ToString();
        editProduct.MinMaxViewModel.WeekdaysMin = selectedProducts.Weekdays.Min;
        editProduct.MinMaxView.MaxTextBox.Text = selectedProducts.Weekdays.Max.ToString();
        editProduct.MinMaxViewModel.WeekdaysMax = selectedProducts.Weekdays.Max;
        editProduct.MinMaxViewModel.EventsMin = selectedProducts.Events.Min;
        editProduct.MinMaxViewModel.EventsMax = selectedProducts.Events.Max;
        editProduct.MinMaxViewModel.WeekendsMin = selectedProducts.Weekends.Min;
        editProduct.MinMaxViewModel.WeekendsMax = selectedProducts.Weekends.Max;

        editProduct.ProductAdded += (product) =>
        {
            AddPopup.IsOpen = true;
            AddProdLabel.Content = "Produto Editado!";
            ContentAddTextBlock.Text = $"O Produto '{product.Name}' foi editado com sucesso!";
        };
        await editProduct.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!.Parent!);
        
    }

    private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        var product = ProductsDataGrid.SelectedItems.Cast<ProductDataGrid>().FirstOrDefault();
        if (product is null) return;
        var selectedProducts = StorageCtrl.FindProduct(product.Gtin);
        
        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Excluir produto do estoque",
            ContentMessage = $"Você realmente deseja excluir \"{selectedProducts.Name}\" do estoque em definitivo?",
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
        StorageCtrl.DeleteProduct(selectedProducts);
        
        Dispatcher.UIThread.Post(() =>
        {
            DeletePopup.IsOpen = true;
            IconDeleteProd.Path = "/Assets/Icons/SVG/IconRemove.svg";
            DeleteProdLabel.Content = "Produto Removido!";
            ContentDeleteTextBlock.Text = $"O '{product.Name}' foi removido do estoque com sucesso!";    
            
        },DispatcherPriority.Background);
        
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