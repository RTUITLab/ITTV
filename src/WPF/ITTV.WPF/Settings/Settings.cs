using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITTV.WPF.DataModel;
using ITTV.WPF.Network.Controll;
using Newtonsoft.Json;

namespace ITTV.WPF.Settings
{
    public class Settings : Singleton<Settings>
    {
        bool configured;

        public event Action SettingsUpdated;
        [JsonProperty]
        bool? needCheckTime;
        [JsonProperty]
        double? sleepHour;
        [JsonProperty]
        bool? isAdmin;
        [JsonProperty]
        double? videoVolume;
        [JsonProperty]
        double? minForUpdate;
        [JsonProperty]
        List<string> backgroundVideoOrder;

        private void GetData()
        {
            if (!configured)
            {
                ConfigControlLogic.Instance.GetSettingsData = () =>
                    new List<ConfigControlLogic.StateControlSetting<object>>()
                    {
                        new ConfigControlLogic.StateControlSetting<object>("needCheckTime",
                            ConfigControlLogic.StateControlSetting<object>.DataTypes.Bool, NeedCheckTime),
                        new ConfigControlLogic.StateControlSetting<object>("sleepHour",
                            ConfigControlLogic.StateControlSetting<object>.DataTypes.Num, sleepHour),
                        new ConfigControlLogic.StateControlSetting<object>("isAdmin",
                            ConfigControlLogic.StateControlSetting<object>.DataTypes.Bool, IsAdmin),
                        new ConfigControlLogic.StateControlSetting<object>("videoVolume",
                            ConfigControlLogic.StateControlSetting<object>.DataTypes.Num, videoVolume),
                        new ConfigControlLogic.StateControlSetting<object>("minForUpdate",
                            ConfigControlLogic.StateControlSetting<object>.DataTypes.Num, MinForUpdate),
                        new ConfigControlLogic.StateControlSetting<object>("backgroundVideoOrder",
                            ConfigControlLogic.StateControlSetting<object>.DataTypes.List, BackgroundVideoOrder),
                    };
                ConfigControlLogic.Instance.SettingFilePath =
                    AppDomain.CurrentDomain.BaseDirectory + @"Settings\settings.json";
                ConfigControlLogic.Instance.SettingsUpdated += GetData;
                configured = true;
            }

            const string path = "Settings/settings.json";
            if (File.Exists(path))
            {
                var data = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));

                instance.backgroundVideoOrder = data?.backgroundVideoOrder;
                instance.needCheckTime = data?.needCheckTime;
                instance.sleepHour = data?.sleepHour;
                instance.isAdmin = data?.isAdmin;
                instance.minForUpdate = data?.minForUpdate;
                instance.videoVolume = data?.videoVolume;

                if (instance.backgroundVideoOrder != null)
                    foreach (var unused in instance.backgroundVideoOrder.Where(uri => !File.Exists(uri)))
                    {
                        instance.backgroundVideoOrder = CreateVideoData().ToList();
                    }

                ConfigControlLogic.Instance.SendSettingsData();
                SettingsUpdated?.Invoke();
            }
        }

        private string[] CreateVideoData()
        {
            string backgroundVideosPath = AppDomain.CurrentDomain.BaseDirectory + @"Videos\Background\";

            if (!Directory.Exists(backgroundVideosPath))
                Directory.CreateDirectory(backgroundVideosPath);

            return Directory.GetFiles(backgroundVideosPath);
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
            get
            {
                if (Instance.backgroundVideoOrder == null) { GetData(); }
                return Instance.backgroundVideoOrder;
            }
        }
    }
}
