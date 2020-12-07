using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.Settings
{
    class Settings
    {
        static Settings(){
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Settings\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string settingsFullPath = fullPath + "settings.json";
            if (!File.Exists(settingsFullPath))
            {
                using (StreamWriter sw = File.CreateText(settingsFullPath))
                {
                    sw.WriteLine("{\n\t\"time\": \"false\",\n\t\"admin\": \"true\",\n\t\"volume\": 0,\n\t\"MinForUpdate\":  5\n}");
                }
            }

            string newsFullPath = fullPath + "news.json";
            if (!File.Exists(newsFullPath))
                File.Create(newsFullPath);
        }

        public static bool NeedCheckTime
        {
            get
            {
                return bool.Parse(JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Settings/settings.json"))["time"]);
            }
        }

        public static bool IsAdmin
        {
            get
            {
                return bool.Parse(JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Settings/settings.json"))["admin"]);
            }
        }

        public static int Volume
        {
            get
            {
                return int.Parse(JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Settings/settings.json"))["volume"]);
            }
        }

        public static int MinForUpdate
        {
            get
            {
                return int.Parse(JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Settings/settings.json"))["MinForUpdate"]);
            }
        }
    }
}
