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
    private Button? _selectedButton;
    private HomeViewModel ViewModel => DataContext as HomeViewModel;
    
    public HomeView()
    {
        InitializeComponent();
    }

    private void unSelectButton()
    {
        _selectedButton?.Classes.Remove("isSelected");
    }

    private void selectButton(Button? btn)
    {
        _selectedButton = btn;
        _selectedButton.Classes.Add("isSelected");
    }

    private void toggleSelectedButton(Button? btn)
    {
        unSelectButton();
        selectButton(btn);
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
    private void btnTrk(object sender, RoutedEventArgs e)
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

    private void StorageView_OnActionChanged(CrudActions actions)
    {
        switch (actions)
        {
            case CrudActions.Create:
                TabStorage.SelectedIndex = 1;
                break;
            case CrudActions.Read:
                TabStorage.SelectedIndex = 1;
                break;
            case CrudActions.Update:
                break;
            case CrudActions.Delete:
                break;
        }
    }

    private void ProdRegisterView_OnProductAdded(Product? product)
    {
        TabStorage.SelectedIndex = 0;
        if (product is null) return;
        ViewModel.Database.AddProduct(product);
        strView?.UpdateStorage();
    }
}