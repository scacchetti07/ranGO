using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarketProject.Models;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class ProdRegisterView : UserControl
{
    public delegate void ProductAddedDelegate(Product? product);
    
    public event ProductAddedDelegate? ProductAdded;
    
    public ProdRegisterViewModel ViewModel => (DataContext as ProdRegisterViewModel)!;
    
    public ProdRegisterView()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        ProductAdded?.Invoke(null);
    }
    
    private void btnAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        Product product = new Product(txtName.Text,int.Parse(txtQtd.Text),txtSup.Text,((cbxStatus.SelectedItem as ComboBoxItem).Content.ToString()),int.Parse(txtMin.Text),int.Parse(txtMax.Text));
        ProductAdded?.Invoke(product);
    }

    private void btnLimpar(object? sender, RoutedEventArgs e)
    {
        txtName.Text = string.Empty;
        txtSup.Text = string.Empty;
        txtQtd.Text = string.Empty;
        txtMin.Text = string.Empty;
        txtMax.Text = string.Empty;
    }
    
}