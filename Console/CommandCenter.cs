using DDSaveBackup.UI;
using DDSaveBackup.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DDSaveBackup.Console
{
    public class CommandCenter
    {
        private List<string> commandList;
        private Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>();

        public CommandCenter()
        {
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
        }
        public void Execute(string command)
        {
            commandList.Add(command);
            var args = Regex.Matches(command, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();
            if (args.Count > 0 && commands.ContainsKey(args[0].ToLower()))
            {
                try
                {
                    commands[args[0]].Invoke(args.ToArray());
                }
                catch
                {
                    Logger.Log(LogLevel.Error,"Exception returns for command");
                }
            }
            else
            {
                Logger.Log(LogLevel.Error,"Invalid command");
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
