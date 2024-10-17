using System.Net.Mime;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarketProject.ViewModels;

namespace MarketProject.Views;

public partial class RegisterMinMaxView : UserControl
{
    public RegisterMinMaxViewModel _vmMinMax => DataContext as RegisterMinMaxViewModel;
    public RegisterMinMaxView()
    {
        InitializeComponent();
        MinTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        MinTextBox.AddHandler(TextBox.KeyDownEvent, KeyDownEvent, RoutingStrategies.Tunnel);
        
        MaxTextBox.AddHandler(TextBox.TextInputEvent, PreviewTextChanged, RoutingStrategies.Tunnel);
        MaxTextBox.AddHandler(TextBox.KeyDownEvent, KeyDownEvent, RoutingStrategies.Tunnel);
        
        SelectedButton = WeekdayButton;
        _vmMinMax.SectionTitle = "Em dias úteis";
    }

    private Button _selectedButton;
    public Button SelectedButton
    {
        get => _selectedButton;
        set
        {
            _selectedButton?.Classes.Clear();
            value.Classes.Add("MinMaxSelected");
            _selectedButton = value;
        }
    }
    
    private void PreviewTextChanged(object sender, TextInputEventArgs e)
    {
        Regex regex = new(@"^[0-9]+$");
        e.Handled = !regex.IsMatch(e.Text!);
    }
    private void KeyDownEvent(object sender, KeyEventArgs e)
    {
        var textBox = sender as TextBox;
        if (e.Key == Key.Back && (textBox.Text.Length == 1 || textBox.SelectedText.Length == textBox.Text.Length))
        {
            textBox.Text = "0";
            e.Handled = true;
        }
    }
    
    // Botões para alterar o conteúdo do text box de acordo com a sessão que for escolhida
    private void WeekdaysButton(object sender, RoutedEventArgs e)
    {
        SelectedButton = WeekdayButton; // Definindo o estilo de seleção no botão
        MinTextBox.Bind(TextBox.TextProperty, new Binding("WeekdaysMin"));
        MaxTextBox.Bind(TextBox.TextProperty, new Binding("WeekdaysMax"));
        _vmMinMax.SectionTitle = "Em dias úteis";
    }

    private void WeekendsButton(object sender, RoutedEventArgs e)
    {
        SelectedButton = WeekendButton;
        MinTextBox.Bind(TextBox.TextProperty, new Binding("WeekendsMin"));
        MaxTextBox.Bind(TextBox.TextProperty, new Binding("WeekendsMax"));
        _vmMinMax.SectionTitle = "Em fins de semana";
    }

    private void EventsButton(object sender, RoutedEventArgs e)
    {
        SelectedButton = EventButton;
        MinTextBox.Bind(TextBox.TextProperty, new Binding("EventsMin"));
        MaxTextBox.Bind(TextBox.TextProperty, new Binding("EventsMax"));
        _vmMinMax.SectionTitle = "Em Eventos";
    }
}