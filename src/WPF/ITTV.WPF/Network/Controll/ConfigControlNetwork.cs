using System;
using System.IO;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Network.Controll.Models;
using Newtonsoft.Json;
using WebSocketSharp;

namespace ITTV.WPF.Network.Controll
{
    public class ConfigControlNetwork
    {
        private readonly WebSocket _socket;
        
        private Configuration _configuration;
        private string _sessionId;

        public ConfigControlNetwork()
        {
            GetConfigControlSettings();

            try
            {
                _socket = new WebSocket("ws://" + _configuration.BaseUrl + "/configs");

                _socket.OnMessage += Socket_OnMessage;
                _socket.OnError += (sender, e) =>
                {
                    ConfigControlLogic.Instance.Log(e.Exception.ToString());
                };

                _socket.Connect();
            } catch (Exception e)
            {
                ConfigControlLogic.Instance.Log(e.Message.Replace(Environment.NewLine, " "));
            }
        }

        private void GetConfigControlSettings()
        {
            var path = AllPaths.FileRfControlConfigurationPath;
            
            if (!File.Exists(path))
                CreateEmptyConfigControlSettings();
            
            _configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
                
            if (string.IsNullOrEmpty(_configuration?.AppName) 
                || string.IsNullOrEmpty(_configuration?.DeviceName)
                || string.IsNullOrEmpty(_configuration.BaseUrl))
            {
                ConfigControlLogic.Instance.Log("Некорректно задана конфигурация RFControl.");
            }
        }

        private void CreateEmptyConfigControlSettings()
        {
            var jsonConfiguration = JsonConvert.SerializeObject(new Configuration());
            File.WriteAllText(AllPaths.FileRfControlConfigurationPath, jsonConfiguration);
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            ConfigControlLogic.Instance.Log("Получение сообщения от сервера.");

            var socketMessage = JsonConvert.DeserializeObject<WebSocketMessage>(e.Data);

            if (string.IsNullOrEmpty(_sessionId))
            {
                _sessionId = socketMessage.SessionID;
            }

            ConfigControlLogic.Instance.UpdateSettingsData(socketMessage.Configs);
        }

        public void SendSettingsToServer(string settings)
        {
            ConfigControlLogic.Instance.Log("Отправка сообщения на сервер.");

            var data = new WebSocketMessage(_configuration.AppName, 
                _configuration.DeviceName, 
                settings, 
                _configuration.AuthToken,
                _sessionId);
            
            if (_socket is {IsAlive: true}) 
            {
                _socket.Send(data.ToJson());
            } 
            else
            {
                ConfigControlLogic.Instance.Log("Отправка не возможна. Нет подключения с сервером по сокету.");
            }
        }
    }
}
