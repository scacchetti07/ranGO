using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MarketProject.Controls;

public partial class ProductCard : UserControl
{
    public static readonly StyledProperty<string> ProdNameProperty =
        AvaloniaProperty.Register<ProductCard, string>(nameof(ProdName));
    public static readonly StyledProperty<int> ProdQtdProperty =
        AvaloniaProperty.Register<ProductCard, int>(nameof(ProdQtd));
    public static readonly StyledProperty<string> SupNameProperty =
    AvaloniaProperty.Register<ProductCard, string>(nameof(SupplyName));
    public static readonly StyledProperty<string> ProdCatProperty =
    AvaloniaProperty.Register<ProductCard, string>(nameof(ProdStatus));
    public static readonly StyledProperty<int> ProdMinProperty =
        AvaloniaProperty.Register<ProductCard, int>(nameof(ProdMin));
    public static readonly StyledProperty<int> ProdMaxProperty =
     AvaloniaProperty.Register<ProductCard, int>(nameof(ProdMax));
    public static readonly StyledProperty<bool> SelectedProperty =
        AvaloniaProperty.Register<ProductCard, bool>(nameof(Selected));
    public static readonly StyledProperty<string> IdProperty =
        AvaloniaProperty.Register<ProductCard, string>(nameof(Id));
    public string ProdName
    {
        get => GetValue(ProdNameProperty);
        set => SetValue(ProdNameProperty, value);
    }
    public int ProdQtd
    {
        get => GetValue(ProdQtdProperty);
        set => SetValue(ProdQtdProperty, value);
    }
    public string SupplyName
    {
        get => GetValue(SupNameProperty);
        set => SetValue(SupNameProperty, value);
    }
    public string ProdStatus
    {
        get => GetValue(ProdCatProperty);
        set => SetValue(ProdCatProperty, value);
    }
    public int ProdMin
    {
        get => GetValue((ProdMinProperty));
        set => SetValue(ProdMinProperty, value);
    }
    public int ProdMax
    {
        get => GetValue((ProdMaxProperty));
        set => SetValue(ProdMaxProperty, value);
    }

    public bool Selected
    {
        get => GetValue(SelectedProperty);
        set => SetValue(SelectedProperty, value);
    }

    public string Id
    {
        get => GetValue(IdProperty);
        set => SetValue(IdProperty, value);
    }

    
    
    public ProductCard()
    {
        InitializeComponent();
        ProdNameProperty.Changed.AddClassHandler<ProductCard>((_, _) => UpdateCard());
        ProdQtdProperty.Changed.AddClassHandler<ProductCard>((_, _) => UpdateCard());
        SupNameProperty.Changed.AddClassHandler<ProductCard>((_, _) => UpdateCard());
        ProdCatProperty.Changed.AddClassHandler<ProductCard>((_, _) => UpdateCard());
        ProdMinProperty.Changed.AddClassHandler<ProductCard>((_, _) => UpdateCard());
        ProdMaxProperty.Changed.AddClassHandler<ProductCard>((_, _) => UpdateCard());
        SelectedProperty.Changed.AddClassHandler<ProductCard>((_, _) => UpdateCard());
    }

    private void UpdateCard()
    {
        txtProductName.Content = ProdName;
        txtQtd.Content = ProdQtd.ToString();
        txtSupply.Content = SupplyName;
        txtCategory.Content = ProdStatus;
        txtMax.Content = ProdMin.ToString();
        txtMin.Content = ProdMin.ToString();
        CardPanel.Classes.Clear();
        if(Selected)
            CardPanel.Classes.Add("SelectedCard");
    }
    
}