using Newtonsoft.Json;

namespace ITTV.WPF.DataModel.Models
{
    public class Configuration
    {
        public Configuration()
        { }

        public Configuration(string appName, 
            string deviceName,
            string authToken, 
            string baseUrl)
        {
            AppName = appName;
            DeviceName = deviceName;
            AuthToken = authToken;
            BaseUrl = baseUrl;
        }
        [JsonProperty("AppName")]
        public string AppName { get; set; }
        [JsonProperty("DeviceName")]
        public string DeviceName { get; set; }
        [JsonProperty("AuthToken")]
        public string AuthToken { get; set; }
        [JsonProperty("BaseURL")]
        public string BaseUrl { get; set; }
    }
}