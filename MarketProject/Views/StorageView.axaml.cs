using System;
using Avalonia;
using MarketProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using MarketProject.Views;
using Avalonia.ReactiveUI;
using MarketProject.Extensions;
using MarketProject.ViewModels;
using ReactiveUI;

namespace MarketProject.Views;

public partial class StorageView : UserControl
{
    // Definindo uma propriedade para o avalonia do tipo produto
    // public static readonly StyledProperty<List<Product>> ProductsProperty =
    //     AvaloniaProperty.Register<StorageView, List<Product>>(nameof(Products));
    
    public delegate void ProductChangedDelegate(Product product);
   // public delegate void SupplyChangedDelegate(Supply supply);
   // public delegate void OrderChangedDelegate(Orders orders);
    public event ProductChangedDelegate ProductChanged;
  //  public event SupplyChangedDelegate SupplyChanged;
   // public event OrderChangedDelegate OrderEvent;
    
    // Necessário para converter o data context para o tipo storageviewmodel, 
    // E implementando suas funcionalidades.
    public StorageViewModel ViewModel => DataContext as StorageViewModel;
    
    // Feito para melhorar a leitura do código a reautilização da mesma função
    // public List<Product> Products
    // {
    //     get => GetValue(ProductsProperty);
    //     set => SetValue(ProductsProperty, value);
    // }
    
    public StorageView()
    {
        InitializeComponent();
        //DataContext = this;
        // // Toda vez que algum produto for adicionad ou alterado no sistema,
        // // O storageView será atualizado com os novos dados adicionados do banco jason para as interface.
        // ProductsProperty.Changed.AddClassHandler<StorageView>((_, _) => UpdateStorage()); 
    }
    
   /* public void UpdateStorage()
    {
        ProductsPanel.Children.Clear(); // limpa todo o paínel visual do sistema
        if (Products is null) return;
        foreach (Product product in Products)
        {
           // lista os produtos do banco json para o painel visual na StorageView
         var productCard = ViewModel.ProductToCard(product);
         pointerpresse = Toda vez que for clicado no card, ele será selecionado ou desselecionado
            productCard.PointerPressed += (sender, args) => 
            {
                 productCard.Selected = !productCard.Selected;
            }; 
            ProductsPanel.Children.Add(productCard);
        }
    
    }
    */
   
    // private void btnRemove_OnClick(object? sender, RoutedEventArgs e)
    // {
    //     ActionChanged?.Invoke(Actions.Delete);
    //     // Ocorre a remoção dos produtos apartir da seleção feita no pointerpressed
    //     IEnumerable<string> selectedProducts = ProductsPanel.Children.Cast<ProductCard>().Where(card => card.Selected)
    //         .Select(card => card.Id);
    //     Products.RemoveAll(product => selectedProducts.Contains(product.Id));
    //     //UpdateStorage();
    // }

    

    private async void RegisterProductButton(object sender, RoutedEventArgs e)
    {
        ProductChanged?.Invoke(new Product());
        
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

    private void ChangeMinMaxTable(object sender, SelectionChangedEventArgs e)
    {
        var prod = new Product();
        var i = SchedComboBox?.SelectedIndex;
        switch (i)
        {
            case 0:
                prod.Min = prod.Weekday.Start;
                prod.Max = prod.Weekday.End;
                break;
            case 1:
                prod.Min = prod.Weekends.Start;
                prod.Max = prod.Weekends.End;
                break;
            case 2:
                prod.Min = prod.Events.Start;
                prod.Max = prod.Events.End;
                break;
        }
    }
}