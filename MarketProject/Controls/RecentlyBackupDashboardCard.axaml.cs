using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Avalonia.Controls;
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
    }

    private void UpdateBackupExplicitday()
    {
        BackupButton.IsEnabled = true;
        string backupPath = @"C:/ranGO/Backup";
        DateTime today = DateTime.Now.Date;
        var lastBackupPath = Directory.GetDirectories(backupPath).Select(d => d.Replace('.', '/')).ToList();
        var backupDays = lastBackupPath.Select(path => path.Remove(0, 16));
        List<int> countingDays = [];
        foreach (var dates in backupDays)
        {
            var backupDate = DateTime.Parse(dates);
            if (today.ToShortDateString() == dates)
            {
                DashboardCardMainContent.Text = "0 DIAS";
                BackupButton.IsEnabled = false;
                break;
            }
            if (today > backupDate)
                countingDays.Add(today.Day - backupDate.Day);
        }

        DashboardCardMainContent.Text = $"{countingDays.Max()} DIAS";
        BackupButton.IsEnabled = true;
    }
}