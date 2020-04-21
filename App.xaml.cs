﻿using System.Windows;
using DDSaveBackup.Log;
using DDSaveBackup.UI;

namespace DDSaveBackup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            Logger.Log(LogLevel.Info,"App started");
            var mainWindow = new MainWindow();
            mainWindow.Show();
            var mainModel = new MainWindowModel(mainWindow);
            mainWindow.SetDataContext(mainModel);
            Logger.MainModel = mainModel;    
            Logger.Log(LogLevel.Info,"App running");
        }
    }
}
