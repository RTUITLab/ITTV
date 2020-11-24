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
