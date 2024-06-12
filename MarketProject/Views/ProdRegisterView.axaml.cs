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
    // Cria um modelo de método tipo ProductAddedDelegate,
    // havendo o parâmetro produto, permitindo valores null
    public delegate void ProductAddedDelegate(Product? product);
    
    // Armazena todos os método criados do tipo ProductAddedDelegate,
    // Executando todas ao mesmo tempo
    public event ProductAddedDelegate? ProductAdded;
    
    // Implementando as funções do ProdRegisterViewmModel por meio do DataContext
    public ProdRegisterViewModel ViewModel => (DataContext as ProdRegisterViewModel)!;
    
    public ProdRegisterView()
    {
        InitializeComponent();
    }

    // Botão de Retornar a tela do estoque
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        // Verifica se existe algum método no ProductAddedDelegate,
        // Caso exista, ele irá enviar null, retornando a última tela.
        ProductAdded?.Invoke(null);
    }
    
    private void btnAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        // É instanciando um objeto do tipo Product,
        // Recebendo todos os campos digitados na View de resgistro,
        Product product = new Product(txtName.Text,int.Parse(txtQtd.Text),txtSup.Text,((cbxStatus.SelectedItem as ComboBoxItem).Content.ToString()),int.Parse(txtMin.Text),int.Parse(txtMax.Text));
        // Caso exista funções em ProductAddedDelegate, ele irá enviar os produtos registrados,
        // Também retornando a tela de estoque.
        ProductAdded?.Invoke(product);
    }

    private void btnLimpar(object? sender, RoutedEventArgs e)
    {
        // Limpa todos os campos de texto após o clique do botão
        txtName.Text = string.Empty;
        txtSup.Text = string.Empty;
        txtQtd.Text = string.Empty;
        cbxStatus.SelectedItem = null;
        txtMin.Text = string.Empty;
        txtMax.Text = string.Empty;
    }
    
}