using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DDSaveBackup.UI;
using DDSaveBackup.Config;

namespace DDSaveBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainWindowModel model;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetDataContext(MainWindowModel model)
        {
            this.model = model;
            DataContext = model;
        }

        public void AppendLog(string message, SolidColorBrush color)
        {
            this.Dispatcher.Invoke(() =>
            {
                var run = new Run(message);
                run.Foreground = color;
                LogsTextBox.Document.Blocks.Add(new Paragraph(run));
                TrimLogs();
            });
        }

        private void TrimLogs()
        {
            var list = LogsTextBox.Document.Blocks;
            while (list.Count > AppConfig.MaxLogLines)
            {
                list.Remove(list.First());
            }
        }

        private void CommandInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                model.Execute();
            }
            else
            {
                model.HandleCommandInputKeys(e.Key);
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            model.Execute();
        }

        private void LogsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogsTextBox.ScrollToEnd();

        }

        private void SaveFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = model.SaveFolderText;
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    model.SaveFolderText = dialog.SelectedPath;
                }
            }
        }

        private void BackUpFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = model.BackUpFolderText;
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    model.BackUpFolderText = dialog.SelectedPath;
                }
            }
        }

        private void RestoreSaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = model.BackUpFolderText;
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    model.RestoreSaveFile(dialog.SelectedPath);
                }
            }
        }

        private void BackUpSaveButton_Click(object sender, RoutedEventArgs e)
        {
            model.BackUpSave();
        }
        private void BackUpToFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = model.BackUpFolderText;
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    model.BackUpSaveToFolder(dialog.SelectedPath);
                }
            }
        }
    }
}
