using System;
using Avalonia;
using MarketProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using MarketProject.Views;
using Avalonia.ReactiveUI;
using MarketProject.ViewModels;
using ReactiveUI;

namespace MarketProject.Views;

public partial class StorageView : UserControl
{
    // Definindo uma propriedade para o avalonia do tipo produto
    public static readonly StyledProperty<List<Product>> ProductsProperty =
        AvaloniaProperty.Register<StorageView, List<Product>>(nameof(Products));
    
    // delegate = cria um modelo que o método deve seguir
    public delegate void ActionChangedDelegate(CrudActions actions);

    // Event armazena todos os método criados do tipo ActionChanged,
    // Executando todas ao mesmo tempo
    public event ActionChangedDelegate ActionChanged;
    
    // Necessário para converter o data context para o tipo storageviewmodel, 
    // E implementando suas funcionalidades.
    public StorageViewModel ViewModel => DataContext as StorageViewModel;

    // Feito para melhorar a leitura do código a reautilização da mesma função
    public List<Product> Products
    {
        get => GetValue(ProductsProperty);
        set => SetValue(ProductsProperty, value);
    }
    
    public StorageView()
    {
        InitializeComponent();
        // this.WhenActivated(action => 
        //     action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        //DataContext = this;
        // // Toda vez que algum produto for adicionad ou alterado no sistema,
        // // O storageView será atualizado com os novos dados adicionados do banco jason para as interface.
        // ProductsProperty.Changed.AddClassHandler<StorageView>((_, _) => UpdateStorage()); 
    }

    // private async Task DoShowDialogAsync(InteractionContext<ProdRegisterViewModel, StorageViewModel?> interaction)
    // {
    //     var dialog = new ProdRegisterView();
    //     dialog.DataContext = interaction.Input;
    //
    //     var result = await dialog.
    //     interaction.SetOutput(result);
    // }
    
    // public void UpdateStorage()
    // {
    //     
    //     ProductsPanel.Children.Clear(); // limpa todo o paínel visual do sistema
    //     if (Products is null) return;
    //     foreach (Product product in Products)
    //     {
    //         // lista os produtos do banco json para o painel visual na StorageView
    //         var productCard = ViewModel.ProductToCard(product);
    //         // pointerpresse = Toda vez que for clicado no card, ele será selecionado ou desselecionado
    //         productCard.PointerPressed += (sender, args) => 
    //         {
    //             productCard.Selected = !productCard.Selected;
    //         }; 
    //         ProductsPanel.Children.Add(productCard);
    //     }
    // }
    
    

    // private void btnNew_OnClick(object? sender, RoutedEventArgs e)
    // {
    //     ActionChanged?.Invoke(CrudActions.Create);
    // }
    //
    // private void btnStorage_OnClick(object? sender, RoutedEventArgs e)
    // {
    //     ActionChanged?.Invoke(CrudActions.Read);
    // }
    //
    // private void btnRemove_OnClick(object? sender, RoutedEventArgs e)
    // {
    //     ActionChanged?.Invoke(CrudActions.Delete);
    //     // Ocorre a remoção dos produtos apartir da seleção feita no pointerpressed
    //     IEnumerable<string> selectedProducts = ProductsPanel.Children.Cast<ProductCard>().Where(card => card.Selected)
    //         .Select(card => card.Id);
    //     Products.RemoveAll(product => selectedProducts.Contains(product.Id));
    //     //UpdateStorage();
    // }
    //
    // private void btnEdit_OnClick(object? sender, RoutedEventArgs e)
    // {
    //     ActionChanged?.Invoke(CrudActions.Update);
    // }


    private async void RegisterProductButton(object sender, RoutedEventArgs e)
    {
        Window prodView = new()
        {
            Title = "Cadastro de Produtos",
            Content = new ProdRegisterView(),
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        prodView.Show();
    }
}

// Ações possíveis de serem feitas no sistema pelo enum
public enum CrudActions
{
    Create,
    Read,
    Update,
    Delete
}