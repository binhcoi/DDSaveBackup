using DDSaveBackup.Log;
using Microsoft.Extensions.Configuration;

namespace DDSaveBackup.Config
{
    public static class AppConfig
    {
        public static int MaxLogLines = 2000;
        public static string BackUpLocation;
        public static string DefaultSaveLocation;
        public static string FileToWatch = "persist.town_event.json";

        public static void Load()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            MaxLogLines = int.Parse(config["MaxLogLines"]);
            BackUpLocation = config["BackUpLocation"];
            DefaultSaveLocation = config["DefaultSaveLocation"];
            FileToWatch = config["FileToWatch"];
            Logger.Log(LogLevel.Info,"Configs loaded");
        }
    }
}