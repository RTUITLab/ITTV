using Newtonsoft.Json;

namespace ITTV.WPF.Network.Controll.Models
{
    public class WebSocketMessage
    {
        public WebSocketMessage() { }
        public WebSocketMessage(string appName, string deviceName, string configs, string authToken, string sessionId = "")
        {
            AppName = appName;
            DeviceName = deviceName;
            SessionID = sessionId;
            Configs = configs;
            AuthToken = authToken;
        }
            
        public string AppName { get; set; }
        public string DeviceName { get; set; }
        public string SessionID { get; set; }
        public string Configs { get; set; }
        public string AuthToken { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}