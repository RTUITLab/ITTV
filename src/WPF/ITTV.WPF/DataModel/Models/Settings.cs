﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ITTV.WPF.DataModel.Models
{ 
    public class Settings : Singleton<Settings>
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
            
            var data = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(AllPaths.FileSettingsPath));

            instance.backgroundVideoOrder = data?.backgroundVideoOrder;
            instance.needCheckTime = data?.needCheckTime;
            instance.sleepHour = data?.sleepHour;
            instance.isAdmin = data?.isAdmin;
            instance.minForUpdate = data?.minForUpdate;
            instance.videoVolume = data?.videoVolume;


            if (instance.backgroundVideoOrder != null 
                && instance.backgroundVideoOrder.Any(uri => !File.Exists(uri)))
            {
                instance.backgroundVideoOrder = GetBackgroundVideos()
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
                var splittingFileName = x.Split('.');
                if (splittingFileName.Length < 2)
                {
                    //TODO: Rewrite logger
                    MainWindow.Log($"The format of the background video file with the name {x} is not specified!");
                    
                    return false;
                }

                var fileFormat = splittingFileName.Last();
                
                var fileSupported = supportedVideoFormats.Contains(fileFormat);
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
