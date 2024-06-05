using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MarketProject.Models;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using MarketProject.Controls;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class StorageView : UserControl
{
    public static readonly StyledProperty<List<Product>> ProductsProperty =
        AvaloniaProperty.Register<StorageView, List<Product>>(nameof(Database));
    
    public delegate void ActionChangedDelegate(CrudActions actions);

    public event ActionChangedDelegate ActionChanged;
    
    public StorageViewModel ViewModel => DataContext as StorageViewModel;

    public List<Product> Products
    {
        get => GetValue(ProductsProperty);
        set => SetValue(ProductsProperty, value);
    }
    
    public StorageView()
    {
        InitializeComponent();
        ProductsProperty.Changed.AddClassHandler<StorageView>((_, _) => UpdateStorage());
    }
    public void UpdateStorage()
    {
        
        ProductsPanel.Children.Clear();
        if (Products is null) return;
        foreach (Product product in Products)
        {
            var productCard = ViewModel.ProductToCard(product);
            productCard.PointerPressed += (sender, args) =>
            {
                productCard.Selected = !productCard.Selected;
            }; 
            ProductsPanel.Children.Add(productCard);
        }
    }
    
    

    private void btnNew_OnClick(object? sender, RoutedEventArgs e)
    {
        ActionChanged?.Invoke(CrudActions.Create);
    }

    private void btnStorage_OnClick(object? sender, RoutedEventArgs e)
    {
        ActionChanged?.Invoke(CrudActions.Read);
    }

    private void btnRemove_OnClick(object? sender, RoutedEventArgs e)
    {
        ActionChanged?.Invoke(CrudActions.Delete);
        IEnumerable<string> selectedProducts = ProductsPanel.Children.Cast<ProductCard>().Where(card => card.Selected)
            .Select(card => card.Id);
        Products.RemoveAll(product => selectedProducts.Contains(product.Id));
        UpdateStorage();
    }

    private void btnEdit_OnClick(object? sender, RoutedEventArgs e)
    {
        ActionChanged?.Invoke(CrudActions.Update);
    }
}

public enum CrudActions
{
    Create,
    Read,
    Update,
    Delete
}