using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups
{
    public class ApiGroupsItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("numbers")]
        public string[] Numbers { get; set; }
    }
}