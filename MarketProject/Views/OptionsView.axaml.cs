using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;

namespace MarketProject.Views;

public partial class OptionsView : UserControl
{
    public OptionsView()
    {
        InitializeComponent();

        if ((ThemeOption.SelectedItem as ComboBoxItem).Content == "Claro")
        {
            
        }
    }
       
    
}