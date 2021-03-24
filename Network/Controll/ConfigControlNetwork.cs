using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Microsoft.Samples.Kinect.ControlsBasics.Network.Controll
{
    class ConfigControlNetwork
    {
        WebSocket socket;
        string BaseUrl = "ws://localhost:8080/AppControl";

        string AppName = "TV";
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


            socket = new WebSocket(BaseUrl);

            socket.OnMessage += Socket_OnMessage;

            socket.Connect();
        }

        private void GetConfigControlSettings()
        {
            string path = "Settings/config_control.json";

            if (File.Exists(path))
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));

                AppName = dict["AppName"];
                DeviceName = dict["DeviceName"];
            }
            else
            {
                CreateConfigControlSettings();
                GetConfigControlSettings();
            }
        }

        private void CreateConfigControlSettings()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Settings\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string settingsFullPath = fullPath + "config_control.json";
            if (!File.Exists(settingsFullPath))
            {
                using (StreamWriter sw = File.CreateText(settingsFullPath))
                {
                    sw.WriteLine("{\n\t\"AppName\": \"ITTV\",\n\t\"DeviceName\": \"ITTV\"\n}");
                }
            }
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            WebSocketMessage socketMessage = JsonConvert.DeserializeObject<WebSocketMessage>(e.Data);

            if (string.IsNullOrEmpty(SessionID))
            {
                SessionID = socketMessage.SessionID;
            }

            ConfigControlLogic.Instance.UpdateSettingsData(socketMessage.Configs);
        }

        public void SendSettingsToServer(string settings)
        {
            WebSocketMessage data = new WebSocketMessage(AppName, DeviceName, settings, SessionID);
            if (socket.IsAlive) {
                socket.Send(data.ToString());
            }
        }
    }
}
