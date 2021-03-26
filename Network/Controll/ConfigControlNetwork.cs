using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using WebSocketSharp;

namespace Microsoft.Samples.Kinect.ControlsBasics.Network.Controll
{
    class ConfigControlNetwork
    {
        WebSocket socket;
        string BaseUrl;

        string AppName;
        string DeviceName;
        string SessionID;

        public class WebSocketMessage
        {
            public string AppName;
            public string DeviceName;
            public string SessionID;
            public string Configs;

            public WebSocketMessage() { }
            public WebSocketMessage(string appName, string deviceName, string configs, string sessionId = "")
            {
                AppName = appName;
                DeviceName = deviceName;
                SessionID = sessionId;
                Configs = configs;
            }

            override
            public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public ConfigControlNetwork()
        {
            if (string.IsNullOrEmpty(AppName) || string.IsNullOrEmpty(DeviceName)) {
                GetConfigControlSettings();
            }

            try
            {
                socket = new WebSocket("ws://" + BaseUrl + "/AppControl");

                socket.OnMessage += Socket_OnMessage;

                socket.Connect();
            } catch (Exception e)
            {
                ConfigControlLogic.Instance.Log(e.Message.Replace(Environment.NewLine, " "));
            }
        }

        private void GetConfigControlSettings()
        {
            string path = "RFControl/configuration.json";

            if (File.Exists(path))
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                
                AppName = dict["AppName"];
                DeviceName = dict["DeviceName"];
                BaseUrl = dict["BaseURL"];

                if (string.IsNullOrEmpty(AppName) || string.IsNullOrEmpty(DeviceName) || string.IsNullOrEmpty(BaseUrl)) {
                    ConfigControlLogic.Instance.Log("Некорректно задана конфигурация RFControl.");
                }
            }
            else
            {
                CreateConfigControlSettings();
                GetConfigControlSettings();
            }
        }

        private void CreateConfigControlSettings()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"RFControl\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string settingsFullPath = fullPath + "configuration.json";
            if (!File.Exists(settingsFullPath))
            {
                using (StreamWriter sw = File.CreateText(settingsFullPath))
                {
                    sw.WriteLine("{\n\t\"AppName\": \"\",\n\t\"DeviceName\": \"\",\n\t\"BaseURL\": \"\"\n}");
                }
            }
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            ConfigControlLogic.Instance.Log("Получение сообщения от сервера.");

            WebSocketMessage socketMessage = JsonConvert.DeserializeObject<WebSocketMessage>(e.Data);

            if (string.IsNullOrEmpty(SessionID))
            {
                SessionID = socketMessage.SessionID;
            }

            ConfigControlLogic.Instance.UpdateSettingsData(socketMessage.Configs);
        }

        public void SendSettingsToServer(string settings)
        {
            ConfigControlLogic.Instance.Log("Отправка сообщения на сервер.");

            WebSocketMessage data = new WebSocketMessage(AppName, DeviceName, settings, SessionID);
            if (socket != null && socket.IsAlive) {
                socket.Send(data.ToString());
            } else
            {
                ConfigControlLogic.Instance.Log("Отправка не возможна. Нет подключения с сервером по сокету.");
            }
        }
    }
}
