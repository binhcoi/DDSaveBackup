using DDSaveBackup.UI;
using System;
using System.Collections.Generic;

namespace DDSaveBackup.Log
{
    public enum LogLevel
    {
        Info,
        Warn,
        Error
    }

    public enum LogType
    {
        Console,
        File,
        Both
    }
    public class Logger
    {
        public static MainWindowModel MainModel;
        public static string SessionFileName;
        private static Queue<Tuple<LogLevel, string>> logsBuffer = new Queue<Tuple<LogLevel, string>>();
        public static void Log(LogLevel level, string message)
        {
            if (MainModel == null)
            {
                logsBuffer.Enqueue(new Tuple<LogLevel, string>(level, message));
                return;
            }
            while (logsBuffer.Count > 0)
            {
                var log = logsBuffer.Dequeue();
                Log(log.Item1, log.Item2);
            }
            var dateTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var logMessage = $"[{dateTimeString} | {level.ToString().ToUpper()}] {message}";
            switch (level)
            {
                case LogLevel.Info:
                    MainModel.AppendInfo(logMessage);
                    break;
                case LogLevel.Warn:
                    MainModel.AppendWarn(logMessage);
                    break;
                case LogLevel.Error:
                    MainModel.AppendError(logMessage);
                    break;
            }
        }
    }
}
