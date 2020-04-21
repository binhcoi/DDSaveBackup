using System.IO;
using DDSaveBackup.Log;

namespace DDSaveBackup.Core
{
    public static class FileManager
    {
        public static FileSystemWatcher Watcher = new FileSystemWatcher();
        public static void WatchFile(string path, string fileName, FileSystemEventHandler action)
        {
            Watcher.Path = path;
            Watcher.NotifyFilter = NotifyFilters.LastWrite;

            Watcher.Filter = fileName;
            // Add event handlers.
            Watcher.Changed += action;
            // Begin watching.
            Watcher.EnableRaisingEvents = true;
            Logger.Log(LogLevel.Info,$"Start watching {path}   {fileName}");
        }        

        public static void CopyFolder(string source, string destination)
        {
            var dir = new DirectoryInfo(source);
            if (!dir.Exists)
            {
                Logger.Log(LogLevel.Error, $"{source} does not exist");
                return;
            }
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var tempPath=Path.Combine(destination,file.Name);
                file.CopyTo(tempPath,true);
            }            
        }

        public static void ClearFolder(string path){
            var dir = new DirectoryInfo(path);
            foreach (var file in dir.EnumerateFiles()){
                file.Delete();
            }
        }


    }
}