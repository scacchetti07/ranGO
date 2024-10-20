using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace MarketProject.Controls;

public partial class FoodCard : UserControl
{
    public static readonly StyledProperty<string> FoodNameProperty =
        AvaloniaProperty.Register<FoodCard, string>(nameof(FoodName));

    public static readonly StyledProperty<string> FoodIngredientsProperty =
        AvaloniaProperty.Register<FoodCard, string>(nameof(FoodIngredients));

    public static readonly StyledProperty<double> FoodPriceProperty =
        AvaloniaProperty.Register<FoodCard, double>(nameof(FoodPrice));

    public static readonly StyledProperty<Bitmap?> FoodPictureProperty =
        AvaloniaProperty.Register<FoodCard, Bitmap?>(nameof(FoodPicture));

    public string FoodName
    {
        get => GetValue(FoodNameProperty);
        set => SetValue(FoodNameProperty, value);
    }

    public string FoodIngredients
    {
        get => GetValue(FoodIngredientsProperty);
        set => SetValue(FoodIngredientsProperty, value);
    }

    public double FoodPrice
    {
        get => GetValue(FoodPriceProperty);
        set => SetValue(FoodPriceProperty, value);
    }

    public Bitmap? FoodPicture
    {
        get => GetValue(FoodPictureProperty);
        set => SetValue(FoodPictureProperty, value);
    }

    public FoodCard()
    {
        InitializeComponent();
        FoodNameProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
        FoodIngredientsProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
        FoodPriceProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
        FoodPictureProperty.Changed.AddClassHandler<FoodCard>((_, _) => UpdateFoodCard());
    }

    private void UpdateFoodCard()
    {
        FoodNameLabel.Content = FoodName;
        FoodIngredientsTextBlock.Text = FoodIngredients;
        FoodPriceTextBlock.Text = FoodPrice.ToString("f2", new CultureInfo("pt-BR")).Insert(0, "R$ ");

        FoodPictureImageBrush.Background = FoodPicture is null
            ? new ImageBrush(new Bitmap("/Assets/DefaultFoodBackground.jpg"))
            : new ImageBrush(FoodPicture);
    }
}