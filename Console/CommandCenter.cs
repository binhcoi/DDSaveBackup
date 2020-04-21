using DDSaveBackup.UI;
using DDSaveBackup.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DDSaveBackup.Core;

namespace DDSaveBackup.Console
{
    public class CommandCenter
    {
        private List<string> commandList;
        private SaveManager saveManager;
        private Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>();

        public CommandCenter(SaveManager manager)
        {
            saveManager = manager;
            commandList = new List<string>();
            commands.Add("log-error", args =>
            {
                Logger.Log(LogLevel.Error, args[1]);
            });
            commands.Add("log-warn", args =>
            {
                Logger.Log(LogLevel.Warn, args[1]);
            });
            commands.Add("log-info", args =>
            {
                Logger.Log(LogLevel.Info, args[1]);
            });
            commands.Add("set-save", args =>
            {
                saveManager.SetSaveLocation(args[1]);
            });
            commands.Add("set-back-up", args =>
            {
                saveManager.SetBackUpLocation(args[1]);
            });
            commands.Add("back-up", args =>
            {
                saveManager.ManualBackup();
            });
            commands.Add("restore", args =>
            {
                saveManager.RestoreSave(args[1]);
            });
            commands.Add("help", args =>
            {
                foreach (var key in commands.Keys)
                {
                    Logger.Log(LogLevel.Info, key);
                }
            });
        }
        public void Execute(string command)
        {
            commandList.Add(command);
            var args = Regex.Matches(command, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value.Replace("\"", string.Empty))
                .ToList();
            if (args.Count > 0 && commands.ContainsKey(args[0].ToLower()))
            {
                try
                {
                    commands[args[0]].Invoke(args.ToArray());
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, "Exception returns for command");
                    Logger.Log(LogLevel.Error, e.Message);
                }
            }
            else
            {
                Logger.Log(LogLevel.Error, "Invalid command");
            }
        }

        public string LastCommand(ref int offset)
        {
            if (commandList.Count == 0 || offset <= 0)
            {
                offset = 0;
                return string.Empty;
            }
            if (offset > commandList.Count)
            {
                offset = commandList.Count;
                return commandList[0];
            }
            return commandList[commandList.Count - offset];
        }
    }
}
