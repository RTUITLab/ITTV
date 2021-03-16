using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
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
        public event Action SettingsUpdated;
        [JsonProperty]
        bool? needCheckTime = null;
        [JsonProperty]
        bool? isAdmin = null;
        [JsonProperty]
        int? videoVolume = null;
        [JsonProperty]
        int? minForUpdate = null;
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
                    sw.WriteLine("{\n\t\"needCheckTime\": true,\n\t\"isAdmin\": true,\n\t\"videoVolume\": 0,\n\t\"minForUpdate\": 5, \n\t\"backgroundVideoOrder\": [");

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

        private void GetData()
        {
            string path = "Settings/settings.json";

            if (File.Exists(path)) {
                instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
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

        public bool IsAdmin
        {
            get
            {
                if (Instance.isAdmin == null) { GetData(); }
                return (bool)Instance.isAdmin;
            }
        }

        public int VideoVolume
        {
            get
            {
                if (Instance.videoVolume == null) { GetData(); }
                return (int)Instance.videoVolume;
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
