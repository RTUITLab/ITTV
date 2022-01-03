using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITTV.WPF.DataModel;
using Newtonsoft.Json;

namespace ITTV.WPF
{
    //TODO: refactoring, not right using concept 
    public class SettingsService : Singleton<SettingsService>
    {
        public event Action SettingsUpdated;
        public bool NeedCheckTime
        {
            get
            {
                if (Instance.needCheckTime == null)
                {
                    UpdateSettings();
                }
                return (bool) Instance.needCheckTime;
            }
        }

        public int SleepHour
        {
            get
            {
                if (Instance.sleepHour == null)
                {
                    UpdateSettings();
                }
                return (int)Instance.sleepHour;
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (Instance.isAdmin == null)
                {
                    UpdateSettings();
                }
                return Instance.isAdmin != null && (bool)Instance.isAdmin;
            }
        }

        public double VideoVolume
        {
            get
            {
                if (Instance.videoVolume == null)
                {
                    UpdateSettings();
                }

                return Math.Min(1, Math.Max(0, Instance.videoVolume.Value) / 100);
            }
        }

        public int MinForUpdate
        {
            get
            {
                if (Instance.minForUpdate == null)
                {
                    UpdateSettings();
                }
                return (int)Instance.minForUpdate;
            }
        }

        public List<string> BackgroundVideoOrder
        {
            get
            {
                if (Instance.backgroundVideoOrder == null)
                {
                    UpdateSettings();
                }
                return Instance.backgroundVideoOrder;
            }
        }
        
        [JsonProperty]
        private bool? needCheckTime;
        [JsonProperty]
        private int? sleepHour;
        [JsonProperty]
        private bool? isAdmin;
        [JsonProperty]
        private int? videoVolume;
        [JsonProperty]
        private int? minForUpdate;
        [JsonProperty]
        private List<string> backgroundVideoOrder;

        private void UpdateSettings()
        {
            if (!File.Exists(AllPaths.FileSettingsPath)) 
                return;
            
            var data = JsonConvert.DeserializeObject<SettingsService>(File.ReadAllText(AllPaths.FileSettingsPath));

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

            SettingsUpdated?.Invoke();
        }

        private static IEnumerable<string> CreateVideoData()
            => Directory.GetFiles(AllPaths.GetDirectoryBackgroundVideosPath);
    }
}
