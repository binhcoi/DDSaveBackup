using System;
using System.IO;
using DDSaveBackup.Log;

namespace DDSaveBackup.Core
{
    public class SaveManager
    {
        private enum BackUpType
        {
            AUTO,
            MANUAL,
            RECOVERY
        }
        private string saveLocation;
        private string backUpLocation;
        private string profile;
        private string fileToWatch;

        public SaveManager(string saveLocation, string backUpLocation, string fileName)
        {
            this.saveLocation = saveLocation;
            profile = new DirectoryInfo(saveLocation).Name;
            this.backUpLocation = backUpLocation;
            fileToWatch = fileName;
            StartWatch();
        }

        public void SetSaveLocation(string path)
        {
            saveLocation = path;
            profile = new DirectoryInfo(path).Name;
            StartWatch();
        }

        public void SetBackUpLocation(string path)
        {
            backUpLocation = path;
        }

        public void SetFileToWatch(string fileName)
        {
            fileToWatch = fileName;
            StartWatch();
        }

        private void StartWatch()
        {
            var dir = new DirectoryInfo(saveLocation);
            if (!dir.Exists)
            {
                Logger.Log(LogLevel.Error, $"{saveLocation} does not exist");
                return;
            }
            if (!File.Exists(Path.Combine(saveLocation, fileToWatch)))
            {
                Logger.Log(LogLevel.Error, $"{fileToWatch} does not exist");
                return;
            }
            FileManager.WatchFile(saveLocation, fileToWatch, OnChanged);
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            Logger.Log(LogLevel.Info, $"File: {e.Name} {e.ChangeType}");
            BackUpSave(BackUpType.AUTO);
        }

        public void ManualBackup()
        {
            BackUpSave(BackUpType.MANUAL);
        }

        public void RecoveryBackUp()
        {
            BackUpSave(BackUpType.RECOVERY);
        }

        public void BackUpSave(string backupFolder)
        {
            try
            {
                FileManager.CopyFolder(saveLocation, backupFolder);
                Logger.Log(LogLevel.Info, $"Save files backed up to {backupFolder}");
            }
            catch (Exception exc)
            {
                Logger.Log(LogLevel.Error, "Error copying file:" + exc.Message);
            }
        }

        private void BackUpSave(BackUpType type)
        {
            var currentTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            var backupFolder = Path.Combine(backUpLocation, $"{profile}__{currentTime}__{type.ToString()}");
            BackUpSave(backupFolder);
        }

        public void RestoreSave(string path)
        {
            RecoveryBackUp();
            try
            {
                FileManager.Watcher.EnableRaisingEvents = false;
                FileManager.ClearFolder(saveLocation);
                FileManager.CopyFolder(path, saveLocation);
                Logger.Log(LogLevel.Info, $"Save file RESTORED from {path}");
                FileManager.Watcher.EnableRaisingEvents = true;
            }
            catch (Exception exc)
            {
                Logger.Log(LogLevel.Error, "Error copying file:" + exc.Message);
            }
        }
    }
}