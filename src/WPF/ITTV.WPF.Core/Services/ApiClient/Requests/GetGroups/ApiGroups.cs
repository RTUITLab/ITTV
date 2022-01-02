using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups
{
    public class ApiGroups
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("groups")]
        public string[] Groups { get; set; }
    }
}