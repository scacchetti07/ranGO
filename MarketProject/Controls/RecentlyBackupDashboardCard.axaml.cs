using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using vmOptions = MarketProject.ViewModels.OptionsViewModel;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;

namespace MarketProject.Controls;

public partial class RecentlyBackupDashboardCard : UserControl
{
    public RecentlyBackupDashboardCard()
    {
        InitializeComponent();
        UpdateBackupExplicitday();
        BackupButton.Click += (_, _) =>
        {
            DashboardCardMainContent.Text = "0 DIAS";
            BackupButton.IsEnabled = false;
        };
        vmOptions.backupAction += (result) =>
        {
            if (!result) return;
            Dispatcher.UIThread.Post(() =>
            {
                DashboardCardMainContent.Text = "0 DIAS";
                BackupButton.IsEnabled = false;
            });
        };
    }

    private void UpdateBackupExplicitday()
    {
        string backupPath = @"C:/ranGO/Backup";
        DateTime today = DateTime.Now.Date;
        var lastBackup = Directory.GetDirectories(backupPath).Select(d => d.Replace('.', '/')).ToList()
            .Select(path => path.Remove(0, 16)).ToArray();
        int dayLimit = 0;
        for (int i = 0; i < lastBackup.Length; i++)
        {
            var backupDate = DateTime.Parse(lastBackup[i]);
            var substractDay = today.Subtract(backupDate).Days;

            if (i == 0)
            {
                dayLimit = substractDay;
                continue;
            }

            if (today == backupDate)
            {
                DashboardCardMainContent.Text = "0 DIAS";
                BackupButton.IsEnabled = false;
                return;
            }
            
            dayLimit = substractDay < dayLimit ? substractDay : dayLimit;
        }

        DashboardCardMainContent.Text = dayLimit == 1 ? $"{dayLimit} DIA" : $"{dayLimit} DIAS";
        BackupButton.IsEnabled = true;
    }
}