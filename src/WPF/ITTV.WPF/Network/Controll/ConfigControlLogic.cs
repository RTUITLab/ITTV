using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ITTV.WPF.Network.Controll
{
    public class ConfigControlLogic
    {
        private static ConfigControlLogic instance = new ConfigControlLogic();
        private static readonly object Padlock = new object();

        public static ConfigControlLogic Instance
        {
            get
            {
                lock (Padlock)
                {
                    return instance;
                }
            }
        }

        private ConfigControlLogic() { }


        private Func<List<StateControlSetting<object>>> _getSettingsData;
        private string _settingsFilePath;
        public event Action SettingsUpdated;
        private ConfigControlNetwork _network;
        public string SettingFilePath { set => _settingsFilePath = value; }

        public Func<List<StateControlSetting<object>>> GetSettingsData
        {
            set => _getSettingsData = value;
        }

        public void SendSettingsData()
        {
            if (_getSettingsData != null) {
                var localSettings = _getSettingsData.Invoke();

                var json = JsonConvert.SerializeObject(localSettings);

                _network ??= new ConfigControlNetwork();

                _network.SendSettingsToServer(json);
            } else
            {
                Log("Не настроенно метод получение конфигурации из приложения в RFControl");
            }
        }

        public void UpdateSettingsData(string response)
        {
            var settings = JsonConvert.DeserializeObject<List<StateControlSetting<object>>>(response);
            var localSettings = new Dictionary<string, object>();

            foreach(StateControlSetting<object> setting in settings)
            {
                if (setting.value != null && !string.IsNullOrEmpty(setting.name)) {
                    if (setting.value is string && setting.type.Equals(StateControlSetting<object>.DataTypes.List.ToString()))
                    {
                        var temp = setting.value.ToString().Replace("[", "").Replace("]", "").Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<object>();
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
                File.WriteAllText(_settingsFilePath, json);
            } catch (Exception)
            {
                if (!string.IsNullOrEmpty(_settingsFilePath) && File.Exists(_settingsFilePath)) {
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
                File.AppendAllLines("RFControl/logs.txt",
                    new[]
                        {DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + log});
            }
            catch (Exception)
            {
                
            }
        }
    }
}
