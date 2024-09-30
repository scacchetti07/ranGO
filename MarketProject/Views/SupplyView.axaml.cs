using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    private Supply _selectedSupply;
    
    public SupplyView()
    {
        InitializeComponent();
        SupplyDataGrid.ItemsSource = Database.SupplyList.Select(SupplyViewModel.SuppliesToDataGrid);
        
        // Resolução do Erro: Call Invalid Thread
        Database.SupplyList.CollectionChanged += (sender, _) =>
        {
            Console.WriteLine((sender as ObservableCollection<Supply>).Count);
            
            // Faz com que o código que atualiza o datagrid seja atualizado na UIThread.
            Dispatcher.UIThread.Post(() =>
            {
                SupplyDataGrid.ItemsSource = new List<SupplyDataGrid>();
                SupplyDataGrid.ItemsSource = (sender as ObservableCollection<Supply>)!
                    .Select(SupplyViewModel.SuppliesToDataGrid);
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
        await addNewSupplyView.ShowDialog((Window)this.Parent!.Parent!.Parent!.Parent!).ConfigureAwait(false);
    }

    private async void DeleteSupply_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selectedSupply is null) return;
        
        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Excluir produto do estoque",
            ContentMessage = $"Você realmente deseja excluir \"{_selectedSupply.Name}\" do estoque em definitivo?",
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
        
        Supplyctrl.DeleteSupply(_selectedSupply);
    }

    private void SupplyDataGrid_OnCellPointerPressed(object sender, DataGridCellPointerPressedEventArgs e)
    {
        var suppliesRow = SupplyDataGrid.SelectedItems.Cast<SupplyDataGrid>();
        foreach (var s in suppliesRow)
            _selectedSupply = Supplyctrl.FindSupply(s.Cnpj);
    }

    private async void EditSupply_OnClick(object sender, RoutedEventArgs e)
    {
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
        try
        {
            List<Product> products = StorageController.FindProductsFromSupply(_selectedSupply);
            editSupply.NameTextBox.Text = _selectedSupply.Name;
            editSupply.CnpjMaskedTextBox.Text = _selectedSupply.Cnpj;
            foreach (var prod in products)
                editSupply.ProductsAutoCompleteBox.Text += $"{prod.Name}, ";
            editSupply.DateLimitTextBox.Text = _selectedSupply.DayLimit.ToString();
            editSupply.CepMaskedTextBox.Text = _selectedSupply.Cep;
            editSupply.AddressTextBox.Text = _selectedSupply.Adress;
            editSupply.EmailTextBox.Text = _selectedSupply.Email;
            editSupply.PhoneMaskedTextBox.Text = _selectedSupply.Phone;
            
            await editSupply.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!).ConfigureAwait(false);
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
}