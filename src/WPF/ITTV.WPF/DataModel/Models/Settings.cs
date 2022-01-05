using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITTV.WPF.Views;
using Newtonsoft.Json;

namespace ITTV.WPF.DataModel.Models
{ 
    public class Settings
    {
        public event Action SettingsUpdated;
        public bool NeedCheckTime
        {
            get
            {
                if (needCheckTime == null)
                {
                    UpdateSettings();
                }
                return (bool) needCheckTime;
            }
        }

        public int SleepHour
        {
            get
            {
                if (sleepHour == null)
                {
                    UpdateSettings();
                }
                return (int) sleepHour;
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (isAdmin == null)
                {
                    UpdateSettings();
                }
                return (bool) isAdmin;
            }
        }

        public double VideoVolume
        {
            get
            {
                if (videoVolume == null)
                {
                    UpdateSettings();
                }

                return Math.Min(1, Math.Max(0, videoVolume.Value) / 100);
            }
        }

        public int MinForUpdate
        {
            get
            {
                if (minForUpdate == null)
                {
                    UpdateSettings();
                }
                return (int) minForUpdate;
            }
        }

        public List<string> BackgroundVideoOrder
        {
            get
            {
                if (backgroundVideoOrder == null)
                {
                    UpdateSettings();
                }
                return backgroundVideoOrder;
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
            
            var data = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(AllPaths.FileSettingsPath));

            backgroundVideoOrder = data?.backgroundVideoOrder;
            needCheckTime = data?.needCheckTime;
            sleepHour = data?.sleepHour;
            isAdmin = data?.isAdmin;
            minForUpdate = data?.minForUpdate;
            videoVolume = data?.videoVolume;


            if (backgroundVideoOrder != null 
                && backgroundVideoOrder.Any(uri => !File.Exists(uri)))
            {
                backgroundVideoOrder = GetBackgroundVideos()
                    .ToList();
            }

            SettingsUpdated?.Invoke();
        }

        private static IEnumerable<string> GetBackgroundVideos()
        {
            var supportedVideoFormats = new[]
            {
                "mov", 
                "ogg",
                "mp4"
            };
            
            var fileNames = Directory.GetFiles(AllPaths.GetDirectoryBackgroundVideosPath);
            
            var filteredFileNames = fileNames.Where(x =>
            {
                var fileExtension = Path.GetExtension(x);
                
                var fileSupported = supportedVideoFormats.Contains(fileExtension);
                if (!fileSupported)
                {
                    //TODO: Rewrite logger
                    MainWindow.Log($"The format of the background video file {x} is not supported");
                }

                return fileSupported;
            });

            return filteredFileNames;
        }
    }
}
