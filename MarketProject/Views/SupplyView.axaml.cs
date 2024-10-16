using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;
using MarketProject.Controllers;
using MarketProject.Models;
using MarketProject.ViewModels;
using MongoDB.Bson;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Supplyctrl = MarketProject.Controllers.SupplyController;

namespace MarketProject.Views;

public partial class SupplyView : UserControl
{
    public SupplyView()
    {
        InitializeComponent();
        SupplyDataGrid.ItemsSource = Database.SupplyList.Select(SupplyViewModel.SuppliesToDataGrid);
        
        // Resolução do Erro: Call Invalid Thread
        Database.SupplyList.CollectionChanged += (sender, _) =>
        {
            // Faz com que o código que atualiza o datagrid seja atualizado na UIThread.
            Dispatcher.UIThread.Post(() =>
            {
                SupplyDataGrid.ItemsSource = new List<SupplyDataGrid>();
                SupplyDataGrid.ItemsSource = (sender as ObservableCollection<Supply>)!
                    .Select(SupplyViewModel.SuppliesToDataGrid);
            }, DispatcherPriority.Background);
        };
        
        Database.ProductsList.CollectionChanged += (_, _) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                SupplyDataGrid.ItemsSource = new List<SupplyDataGrid>();
                SupplyDataGrid.ItemsSource = Database.SupplyList.Select(SupplyViewModel.SuppliesToDataGrid);
            }, DispatcherPriority.Background);
        };
    }

    private async void AddSupply_OnClick(object sender, RoutedEventArgs e)
    {
        SupplyAddView addNewSupplyView = new()
        {
            Title = "Cadastro de Fornecedores",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        addNewSupplyView.SupplyAdded += (supply) =>
        {
            AddPopup.IsOpen = true;
            AddProdLabel.Content = "Novo Fornecedor Adicionado!";
            ContentAddTextBlock.Text = $"O Fornecedor '{supply.Name}' foi adicionado com sucesso!";
        };
        await addNewSupplyView.ShowDialog((Window)this.Parent!.Parent!.Parent!.Parent!).ConfigureAwait(false);
    }

    private async void DeleteSupply_OnClick(object sender, RoutedEventArgs e)
    {
        var supplies = SupplyDataGrid.SelectedItems.Cast<SupplyDataGrid>().FirstOrDefault();
        var selectedSupply = Supplyctrl.FindSupply(supplies.Cnpj);
        
        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Excluir produto do estoque",
            ContentMessage = $"Você realmente deseja excluir \"{selectedSupply.Name}\" do estoque em definitivo?",
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
        Supplyctrl.DeleteSupply(selectedSupply);
        
        Dispatcher.UIThread.Post(() =>
        {
            DeletePopup.IsOpen = true;
            DeleteProdLabel.Content = "Fornecedor Removido!";
            ContentDeleteTextBlock.Text = $"O '{selectedSupply.Name}' foi removido do estoque com sucesso!";
            
        }, DispatcherPriority.Background);
        
    }

    private async void EditSupply_OnClick(object sender, RoutedEventArgs e)
    {
        var supplies = SupplyDataGrid.SelectedItems.Cast<SupplyDataGrid>().FirstOrDefault();
        if (supplies is null) return;
        var selectedSupply = Supplyctrl.FindSupply(supplies.Cnpj);
        
        SupplyAddView editSupply = new()
        {
            Title = "Cadastro de Fornecedores",
            WindowStartupLocation= WindowStartupLocation.CenterScreen,
            ExtendClientAreaChromeHints= ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaToDecorationsHint = true,
            CanResize = false,
            ShowInTaskbar = false,
            SizeToContent = SizeToContent.WidthAndHeight
        };
        editSupply.AddButton.Content = "Editar";
        
        List<Product> products = StorageController.FindProductsFromSupply(selectedSupply);
        editSupply.NameTextBox.Text = selectedSupply.Name;
        var editedCnpj = selectedSupply.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
        
        editSupply.CnpjMaskedTextBox.Text = editedCnpj;
        editSupply.CnpjMaskedTextBox.IsEnabled = false;
        foreach (var prod in products)
        {
            editSupply.AutoCompleteSelectedProducts.Add(prod);
            editSupply.TagContentStackPanel.Children.Add(editSupply.GenereteAutoCompleteTag(prod));
            
            var itemSource = editSupply.ProductsAutoCompleteBox.ItemsSource.Cast<string>().ToList();
            itemSource.Remove(prod.Name);
            editSupply.ProductsAutoCompleteBox.ItemsSource = itemSource;
        }
        editSupply.DateLimitTextBox.Text = selectedSupply.DayLimit.ToString();
        editSupply.CepMaskedTextBox.Text = selectedSupply.Cep.Replace("-", "");
        editSupply.AddressTextBox.Text = selectedSupply.Adress;
        editSupply.EmailTextBox.Text = selectedSupply.Email;
        editSupply.PhoneMaskedTextBox.Text = selectedSupply.Phone;

        editSupply.SupplyAdded += (supply) =>
        {
            if (supply is null) return;
            AddPopup.IsOpen = true;
            AddProdLabel.Content = "Fornecedor Editado!";
            ContentAddTextBlock.Text = $"O Fornecedor '{supply.Name}' foi editado com sucesso!";
        };
        await editSupply.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!).ConfigureAwait(false);
    }

    private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = SearchTextBox.Text;
        if (keyword.Length < 1)
        {
            SupplyDataGrid.ItemsSource = Database.SupplyList!
                .Select(SupplyViewModel.SuppliesToDataGrid);
            return;
        }

        var regexPattern = new Regex("@[./-]|\\d");

        IEnumerable<Supply> searchedList;
        if (regexPattern.IsMatch(keyword))
            searchedList = Database.SupplyList.Where(p => p.Cnpj.Contains(keyword));
        else
            searchedList = Database.SupplyList.Where(p => p.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            
        SupplyDataGrid.ItemsSource = searchedList!.Select(SupplyViewModel.SuppliesToDataGrid);
    }
}