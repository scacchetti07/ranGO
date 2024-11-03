using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Newtonsoft.Json;
using db = MarketProject.Models.Database;

namespace MarketProject.ViewModels;

public class OptionsViewModel : ViewModelBase
{
    public ICommand AccessWebSite { get; }
    public ICommand SentAMailTo { get; }
    public ICommand DoRangoBackup { get; }
    public static event Action<bool> backupAction;

    private const string _light = "Claro";
    private const string _dark = "Escuro";
    private string _currentAppTheme;

    public string CurrentAppTheme
    {
        get => _currentAppTheme;
        set
        {
            var theme = this.RaiseAndSetIfChanged(ref _currentAppTheme, value);
            if (GetTheme(theme) == null) return;
            Application.Current.RequestedThemeVariant = GetTheme(theme);
        }
    }

    public OptionsViewModel()
    {
        // Definindo a lista de temas e o padrão quando for iniciado
        Themes = new ObservableCollection<string> { _dark, _light };
        DefaultThemes();

        AccessWebSite = ReactiveCommand.Create(() =>
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://pietromauergodoy.github.io/ranGO_WebSite/",
                UseShellExecute = true
            };
            Process.Start(psi);
        });

        SentAMailTo = ReactiveCommand.Create(SentEmail);
        DoRangoBackup = ReactiveCommand.Create(BackupFiles);
    }

    public void DefaultThemes()
    {
        CurrentAppTheme = _dark;
    }

    private void SentEmail()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "mailto:rangoemp@gmail.com",
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    public ObservableCollection<string> Themes { get; }

    public ThemeVariant GetTheme(string value)
    {
        return value switch
        {
            _light => ThemeVariant.Light,
            _dark => ThemeVariant.Dark,
            _ => null
        };
    }

    public async void BackupFiles()
    {
        string Today = DateTime.Now.ToShortDateString().Replace('/', '.');
        List<string> jsonLists = new()
        {
            JsonConvert.SerializeObject(db.SupplyList), JsonConvert.SerializeObject(db.ProductsList),
            JsonConvert.SerializeObject(db.OrdersList), JsonConvert.SerializeObject(db.FoodsMenuList)
        };
        var backupFilesName = new List<string>
        {
            "supplys.json", "products.json",
            "orders.json", "foodMenu.json"
        };

        for (int i = 0; i < backupFilesName.Count; i++)
        {
            string dirPath = $@"C:\ranGO\Backup\{Today}\";
            string filePath = dirPath + @$"\{backupFilesName[i]}";
            if (File.Exists(filePath))
                File.Delete(filePath);
            else
                Directory.CreateDirectory(dirPath);
            await using StreamWriter sw = new(filePath);
            await sw.WriteLineAsync(jsonLists[i]).ConfigureAwait(false);
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
        OnBackupAction(true);
    }

    protected virtual void OnBackupAction(bool obj)
    {
        backupAction?.Invoke(obj);
    }
}