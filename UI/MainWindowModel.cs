using System.Windows.Input;
using System.Windows.Media;
using DDSaveBackup.Config;
using DDSaveBackup.Console;
using DDSaveBackup.Core;

namespace DDSaveBackup.UI
{
    public class MainWindowModel : Bindable
    {
        private CommandCenter commandCenter;
        private SaveManager saveManager;
        private MainWindow mainWindow;
        private int commandOffset = 0;
        public MainWindowModel(MainWindow window)
        {
            saveManager = new SaveManager(AppConfig.DefaultSaveLocation, AppConfig.BackUpLocation, AppConfig.FileToWatch);
            saveFolderText = AppConfig.DefaultSaveLocation;
            backupFolderText = AppConfig.BackUpLocation;
            commandCenter = new CommandCenter(saveManager);

            mainWindow = window;
            CommandInputText = string.Empty;
        }
        private string commandInputText;
        public string CommandInputText
        {
            get { return commandInputText; }
            set
            {
                SetProperty(ref commandInputText, value);
            }
        }

        private string saveFolderText;
        public string SaveFolderText
        {
            get { return saveFolderText; }
            set
            {
                SetProperty(ref saveFolderText, value);
                saveManager.SetSaveLocation(saveFolderText);
            }
        }

        private string backupFolderText;
        public string BackUpFolderText
        {
            get { return backupFolderText; }
            set
            {
                SetProperty(ref backupFolderText, value);
                saveManager.SetBackUpLocation(backupFolderText);
            }
        }

        public void RestoreSaveFile(string path)
        {
            saveManager.RestoreSave(path);
        }

        public void BackUpSave()
        {
            saveManager.ManualBackup();
        }

        public void BackUpSaveToFolder(string path)
        {
            saveManager.BackUpSave(path);
        }

        public void Execute()
        {
            commandCenter.Execute(CommandInputText);
            commandOffset = 0;
            CommandInputText = "";
        }

        public void HandleCommandInputKeys(Key key)
        {
            if (key == Key.Up)
            {
                commandOffset++;
                CommandInputText = commandCenter.LastCommand(ref commandOffset);
            }
            else if (key == Key.Down)
            {
                commandOffset--;
                CommandInputText = commandCenter.LastCommand(ref commandOffset);
            }
        }

        public void AppendInfo(string message)
        {
            mainWindow.AppendLog(message, Brushes.Black);
        }
        public void AppendWarn(string message)
        {
            mainWindow.AppendLog(message, Brushes.DarkOrange);
        }
        public void AppendError(string message)
        {
            mainWindow.AppendLog(message, Brushes.Red);
        }
    }
}