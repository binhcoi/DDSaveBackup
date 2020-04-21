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
            saveManager = new SaveManager(AppConfig.DefaultSaveLocation,AppConfig.BackUpLocation, AppConfig.FileToWatch);
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