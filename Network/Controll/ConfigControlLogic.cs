using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.Network.Controll
{
    public class ConfigControlLogic
    {
        private static ConfigControlLogic instance = new ConfigControlLogic();
        private static readonly object padlock = new object();

        public static ConfigControlLogic Instance
        {
            get
            {
                lock (padlock)
                {
                    return instance;
                }
            }
        }

        private ConfigControlLogic() { }


        Func<List<StateControlSetting<object>>> getSettingsData = null;
        public event Action SettingsUpdated;
        private ConfigControlNetwork network;

        public Func<List<StateControlSetting<object>>> GetSettingsData
        { 
            set
            {
                this.getSettingsData = value;
            }
        }

        public async void SendSettingsData()
        {
            if (getSettingsData != null) {
                List<StateControlSetting<object>> localSettings = getSettingsData.Invoke();

                string json = JsonConvert.SerializeObject(localSettings);

                if (network == null)
                {
                    network = new ConfigControlNetwork();
                }

                network.SendSettingsToServer(json);
            }
        }

        public void UpdateSettingsData(string response)
        {
            List<StateControlSetting<object>> settings = JsonConvert.DeserializeObject<List<StateControlSetting<object>>>(response);
            Dictionary<string, object> localSettings = new Dictionary<string, object>();

            foreach(StateControlSetting<object> setting in settings)
            {
                if (setting.value.GetType() == typeof(string) && setting.type.Equals(StateControlSetting<object>.DataTypes.List.ToString()))
                {
                    List<object> temp = setting.value.ToString().Replace("[", "").Replace("]", "").Split(new char[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries).ToList<object>();
                    localSettings.Add(setting.name, temp);
                } else {
                    localSettings.Add(setting.name, setting.value);
                }
            }

            string json = JsonConvert.SerializeObject(localSettings);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Settings\settings.json", json);

            SettingsUpdated?.Invoke();
        }

        public class StateControlSetting<T>
        {
            public enum DataTypes
            {
                Bool,
                List,
                Text,
                Num
            }

            public string name;
            public string type;
            public T value;

            public StateControlSetting(string name, DataTypes type, T value)
            {
                this.name = name;
                this.value = value;
                this.type = type.ToString();
            }

        }
    }
}
