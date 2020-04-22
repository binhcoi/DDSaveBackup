using System.Windows;
using DDSaveBackup.Log;
using DDSaveBackup.UI;
using DDSaveBackup.Config;

namespace DDSaveBackup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            AppConfig.Load();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            var mainModel = new MainWindowModel(mainWindow);
            mainWindow.SetDataContext(mainModel);
            Logger.MainModel = mainModel;
            Logger.Log(LogLevel.Info, "App started");

        }
    }
}
