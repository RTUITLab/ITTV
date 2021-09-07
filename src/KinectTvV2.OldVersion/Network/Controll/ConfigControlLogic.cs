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
        string settingsFilePath;
        public event Action SettingsUpdated;
        private ConfigControlNetwork network;
        public string SettingFilePath { set => settingsFilePath = value; }
        public Func<List<StateControlSetting<object>>> GetSettingsData { set => this.getSettingsData = value; }

        public void SendSettingsData()
        {
            if (getSettingsData != null) {
                List<StateControlSetting<object>> localSettings = getSettingsData.Invoke();

                string json = JsonConvert.SerializeObject(localSettings);

                if (network == null)
                {
                    network = new ConfigControlNetwork();
                }

                network.SendSettingsToServer(json);
            } else
            {
                Log("Не настроенно метод получение конфигурации из приложения в RFControl");
            }
        }

        public void UpdateSettingsData(string response)
        {
            List<StateControlSetting<object>> settings = JsonConvert.DeserializeObject<List<StateControlSetting<object>>>(response);
            Dictionary<string, object> localSettings = new Dictionary<string, object>();

            foreach(StateControlSetting<object> setting in settings)
            {
                if (setting.value != null && !string.IsNullOrEmpty(setting.name)) {
                    if (setting.value.GetType() == typeof(string) && setting.type.Equals(StateControlSetting<object>.DataTypes.List.ToString()))
                    {
                        List<object> temp = setting.value.ToString().Replace("[", "").Replace("]", "").Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<object>();
                        localSettings.Add(setting.name, temp);
                    } else {
                        localSettings.Add(setting.name, setting.value);
                    }
                } else
                {
                    Log($"Получено значение null в поле конфигурации {setting.name}.");
                }
            }

            string json = JsonConvert.SerializeObject(localSettings);
            try
            {
                File.WriteAllText(settingsFilePath, json);
            } catch (Exception)
            {
                if (!string.IsNullOrEmpty(settingsFilePath) && File.Exists(settingsFilePath)) {
                    Log("Нет доступа к файлу конфигурации приложения.");
                } else
                {
                    Log("Не задан путь к конфигурационному файлу приложения");
                }
            }

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

        public void Log(string log)
        {
            try
            {
                File.AppendAllLines("RFControl/logs.txt", new string[] { DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + log });
            }
            catch (Exception) { }
        }
    }
}
