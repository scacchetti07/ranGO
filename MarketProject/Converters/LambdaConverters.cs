using Avalonia.Data.Converters;

namespace MarketProject.Converters;

public class LambdaConverters
{
    public static FuncValueConverter<string, string> ProdConverter { get; } =
        new FuncValueConverter<string, string>(valor => valor == null || valor.Trim() == string.Empty ? "Novo Produto" : valor);
}