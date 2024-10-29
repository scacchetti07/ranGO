using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ZstdSharp.Unsafe;

namespace MarketProject.Controls;

public partial class ImportDataPopUp : Window
{
    private FilePickerFileType[] _fileType { get; } = new FilePickerFileType[]
    {
        new("Todos")
        {
            Patterns = new[] { ".xlsx", ".ods" ,".json" },
            MimeTypes = new[]
            {
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "application/x-vnd.oasis.opendocument.spreadsheet",
                "application/vnd.google-apps.script+json"
            }
        },
        new("Microsoft Excel")
        {
            Patterns = new[] { "*.xlsx" },
            MimeTypes = new[]
            {
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            }
        },
        new("OpenDocument")
        {
            Patterns = new[] { "*.ods" },
            MimeTypes = new[] { "application/x-vnd.oasis.opendocument.spreadsheet" }
        },
        new("JSON")
        {
            Patterns = new[] { "*.json" },
            MimeTypes = new[] { "application/vnd.google-apps.script+json" }
        },
    };

    public delegate void ImportOptionSelected(IStorageFile file, bool isBackup = false);

    public event ImportOptionSelected ImportOption;

    public ImportDataPopUp()
    {
        InitializeComponent();
    }

    private void ExitPopUpButton_OnClick(object sender, RoutedEventArgs e)
    {
        ImportOption?.Invoke(null);
        Close();
    }


    private void ImportByBackupButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Aplicar import automático das tabelas a partir dos arquivos .json já na pasta, alterando diretamente o banco e em seguida atualizando as tabelas.
        ImportOption?.Invoke(null, true);
        Close();
    }

    private async void ImportByFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Esperar o arquivo do usuário e depois adaptar para a tabela e dps enviar para o banco de dados.
        FilePickerOpenOptions fileoption = new()
        {
            AllowMultiple = false,
            Title = "Selecione um arquivo para importar",
            FileTypeFilter = _fileType
        };
        var fileList = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(fileoption).ConfigureAwait(false);
        foreach (var storageFile in fileList)
            ImportOption?.Invoke(storageFile);
    }
}