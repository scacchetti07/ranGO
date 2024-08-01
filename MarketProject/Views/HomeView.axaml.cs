using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarketProject.Models;
using MarketProject.ViewModels;
using ReactiveUI;

namespace MarketProject.Views;

public partial class HomeView : Window
{
    // variável do tipo botão indicando que já ta selecionado
    private Button? _selectedButton;

    // Chamando as funcionalidades do HomeViewModel
    private HomeViewModel ViewModel => DataContext as HomeViewModel;
    
    public HomeView()
    {
        InitializeComponent();
        
    }

    // Método que verifica se a variável do botão selecionado é nula,
    // Caso for verdade, ele retorna nada, se não for, ele remove o estilo de selecionado
    private void unSelectButton()
    {
        _selectedButton?.Classes.Remove("isSelected");
    }

    // Método que adiciona uma seleção visual no botão clicado
    private void selectButton(Button? btn)
    {
        _selectedButton = btn;
        _selectedButton.Classes.Add("isSelected");
    }

    // Método que verifica se aquele botão 
    private void toggleSelectedButton(Button? btn)
    {
        unSelectButton(); // ele remove o estilo caso a variável _selectedButton tiver algum valor
        selectButton(btn); // ele adiciona o estilo de selecionado ao botão clicado
    }
    
    private void btnDashb(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 1;
        toggleSelectedButton(sender as Button);
    }
    private void btnPack(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 2;
        toggleSelectedButton(sender as Button);
    }
    private void BtnSupply(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 3;
        toggleSelectedButton(sender as Button);
    }

    private void btnEmp(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 4;
        toggleSelectedButton(sender as Button);
    }

    private void btnGrp(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 5;
        toggleSelectedButton(sender as Button);
    }

    private void btnSett(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 6;
        toggleSelectedButton(sender as Button);
    }

    private void btnPrf(object sender, RoutedEventArgs e)
    {
        TabGeral.SelectedIndex = 7;
        toggleSelectedButton(sender as Button);
    }

    // Método que indica as ações fornecidas dentro do user control do storage View.
    // Opções presentes no enum do StorageView.
    private void StorageView_OnActionChanged(CrudActions actions)
    {
        switch (actions)
        {
            case CrudActions.Create:
                TabStorage.SelectedIndex = 1;
                break;
            case CrudActions.Read:
                break;
            case CrudActions.Update:
                TabStorage.SelectedIndex = 1;
                break;
            case CrudActions.Delete:
                break;
        }
    }

    // Método que registra os produtos digitados no user control para o banco de dados e para StorageView
    private void ProdRegisterView_OnProductAdded(Product? product)
    {
        TabStorage.SelectedIndex = 0;
        if (product is null) return;
        ViewModel.Database.AddProduct(product); // Adiciona o produto digitado no banco json
        // strView?.UpdateStorage(); // Atualiza na tela StorageView o estoque atual
    }
}