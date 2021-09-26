using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetGroups
{
    public class ApiGroups
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("groups")]
        public string[] Groups { get; set; }
    }
}