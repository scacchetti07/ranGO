using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MarketProject.Views;

public partial class RemoveProductView : Window
{
    public RemoveProductView()
    {
        InitializeComponent();
        
        RemoveTotalTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
    }

    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }
    
    private void RemoveProduct(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ReturnStorage(object sender, RoutedEventArgs e) => this.Close();
}