using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using Microsoft.Samples.Kinect.ControlsBasics.Network.Controll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.TVSettings
{
    public class Settings : Singleton<Settings>
    {
        bool configured = false;

        public event Action SettingsUpdated;
        [JsonProperty]
        bool? needCheckTime = null;
        [JsonProperty]
        double? sleepHour = null;
        [JsonProperty]
        bool? isAdmin = null;
        [JsonProperty]
        double? videoVolume = null;
        [JsonProperty]
        double? minForUpdate = null;
        [JsonProperty]
        List<string> backgroundVideoOrder = null;

        private void CreateData(){
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Settings\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string settingsFullPath = fullPath + "settings.json";
            if (!File.Exists(settingsFullPath))
            {
                using (StreamWriter sw = File.CreateText(settingsFullPath))
                {
                    sw.WriteLine("{\n\t\"needCheckTime\": true,\n\t\"sleepHour\": 22,\n\t\"isAdmin\": true,\n\t\"videoVolume\": 0,\n\t\"minForUpdate\": 5, \n\t\"backgroundVideoOrder\": [");

                    string backgroundVideosPath = AppDomain.CurrentDomain.BaseDirectory + @"Videos\Background\";

                    if (!Directory.Exists(backgroundVideosPath))
                        Directory.CreateDirectory(backgroundVideosPath);

                    string[] AllFiles = Directory.GetFiles(backgroundVideosPath);

                    foreach (string video in AllFiles)
                    {
                        sw.WriteLine("\t\t\"" + video.Replace("\\", "\\\\") + "\",");
                    }
                    sw.WriteLine("\t]\n}");
                }
            }

            string newsFullPath = fullPath + "news.json";
            if (!File.Exists(newsFullPath))
                File.Create(newsFullPath);
        }

        public void GetData()
        {
            if (!configured) {
                ConfigControlLogic.Instance.GetSettingsData = () => {
                    return new List<ConfigControlLogic.StateControlSetting<object>>()
                    {
                        new ConfigControlLogic.StateControlSetting<object>("needCheckTime", ConfigControlLogic.StateControlSetting<object>.DataTypes.Bool, NeedCheckTime),
                        new ConfigControlLogic.StateControlSetting<object>("sleepHour", ConfigControlLogic.StateControlSetting<object>.DataTypes.Num, sleepHour),
                        new ConfigControlLogic.StateControlSetting<object>("isAdmin", ConfigControlLogic.StateControlSetting<object>.DataTypes.Bool, IsAdmin),
                        new ConfigControlLogic.StateControlSetting<object>("videoVolume", ConfigControlLogic.StateControlSetting<object>.DataTypes.Num, VideoVolume),
                        new ConfigControlLogic.StateControlSetting<object>("minForUpdate", ConfigControlLogic.StateControlSetting<object>.DataTypes.Num, MinForUpdate),
                        new ConfigControlLogic.StateControlSetting<object>("backgroundVideoOrder", ConfigControlLogic.StateControlSetting<object>.DataTypes.List, BackgroundVideoOrder),
                    };
                };
                ConfigControlLogic.Instance.SettingFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Settings\settings.json";
                ConfigControlLogic.Instance.SettingsUpdated += GetData;
                configured = !configured;
            }

            string path = "Settings/settings.json";

            if (File.Exists(path)) {
                instance.backgroundVideoOrder = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path)).backgroundVideoOrder;
                instance.needCheckTime = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path)).needCheckTime;
                instance.sleepHour = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path)).sleepHour;
                instance.isAdmin = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path)).isAdmin;
                instance.minForUpdate = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path)).minForUpdate;
                instance.videoVolume = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path)).videoVolume;
                ConfigControlLogic.Instance.SendSettingsData();
                SettingsUpdated?.Invoke();
            } else
            {
                CreateData();
                GetData();  
            }
        }

        public bool NeedCheckTime
        {
            get
            {
                if (Instance.needCheckTime == null) { GetData(); }
                return (bool)Instance.needCheckTime;
            }
        }

        public int SleepHour
        {
            get
            {
                if (Instance.sleepHour == null) { GetData(); }
                return (int)Instance.sleepHour;
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (Instance.isAdmin == null) { GetData(); }
                return (bool)Instance.isAdmin;
            }
        }

        public double VideoVolume
        {
            get
            {
                if (Instance.videoVolume == null) { GetData(); }
                if (Instance.videoVolume > 100) { return 1; } else if (Instance.videoVolume < 0) { return 0; } else { return (double)Instance.videoVolume / 100; }
            }
        }

        public int MinForUpdate
        {
            get
            {
                if (Instance.minForUpdate == null) { GetData(); }
                return (int)Instance.minForUpdate;
            }
        }

        public List<string> BackgroundVideoOrder
        {
            get {
                if (Instance.backgroundVideoOrder == null) { GetData(); }
                return Instance.backgroundVideoOrder;
            }
        }
    }
}
