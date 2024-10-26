using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MarketProject.Controls;

public partial class ImportDataPopUp : Window
{
    public ImportDataPopUp()
    {
        InitializeComponent();
    }

    private void ExitPopUpButton_OnClick(object sender, RoutedEventArgs e) =>
        Close();

    private void ImportByBackupButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Aplicar import automático das tabelas a partir dos arquivos .json já na pasta, alterando diretamente o banco e em seguida atualizando as tabelas.
    }

    private void ImportByFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Esperar o arquivo do usuário e depois adaptar para a tabela e dps enviar para o banco de daods.
    }
}