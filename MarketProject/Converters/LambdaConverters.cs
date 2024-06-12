using Avalonia.Data.Converters;

namespace MarketProject.Converters;

public class LambdaConverters
{
    // Esse Método estático Permite que visualmente quando o nome do produto é digitado no campo,
    // Ele será escrito no título acima de "Novo Produto", caso for vazio ele terá outro nome definido.
    // Método implementado no XAML do Registro de produtos.
    public static FuncValueConverter<string, string> ProdConverter { get; } =
        new FuncValueConverter<string, string>(valor => valor == null || valor.Trim() == string.Empty ? "Novo Produto" : valor);
}