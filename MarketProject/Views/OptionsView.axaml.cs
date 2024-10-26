using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using MarketProject.ViewModels;
using System.Diagnostics;
using db = MarketProject.Models.Database;
using System.IO;
using MarketProject.Controls;
using MongoDB.Bson;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;


namespace MarketProject.Views;

public partial class OptionsView : UserControl
{
    private const string _backupPath = @"C:\ranGO\Backup";

    private readonly List<string> _backupFilesName = new()
    {
        "products.json", "supplys.json",
        "orders.json", "foodMenu.json"
    };

    public OptionsView()
    {
        InitializeComponent();
        Directory.CreateDirectory(@"C:\ranGO\Backup"); // Cria o diretório no Disco C caso o diretório não existir.
    }

    private async void BackupButton_OnClick(object sender, RoutedEventArgs e)
    {
        List<string> jsonLists = new()
        {
            JsonConvert.SerializeObject(db.ProductsList), JsonConvert.SerializeObject(db.SupplyList),
            JsonConvert.SerializeObject(db.OrdersList), JsonConvert.SerializeObject(db.FoodsMenuList)
        };

        for (int i = 0; i < _backupFilesName.Count; i++)
        {
            string path = $@"{_backupPath}\{_backupFilesName[i]}";
            await using var file = File.AppendText(path);
            await file.WriteLineAsync(jsonLists[i]).ConfigureAwait(false);
        }

        var msgBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Backup realizado com Sucesso!",
            ContentMessage = "O Backup dos dados registrados no software ranGO! foram salvos!",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Success,
            CanResize = false,
            ShowInCenter = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly
        });
        await msgBox.ShowAsync().ConfigureAwait(false);
    }

    private void ImportButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Perguntar sobre se quer importar os dados do backup ou arquivos manuais
        ImportDataPopUp importDataPopUp = new ImportDataPopUp();
        importDataPopUp.ShowDialog((Window)Parent!.Parent!.Parent!.Parent!);
    }
}

public enum FileType
{
    xlsx,
    csv,
    json
}