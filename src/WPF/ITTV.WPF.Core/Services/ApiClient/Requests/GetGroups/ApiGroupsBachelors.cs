using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups
{
    public class ApiGroupsBachelors
    {
        [JsonProperty("first")]
        public ApiGroupsItem[] First { get; set; }

        [JsonProperty("fourth")]
        public ApiGroupsItem[] Fourth { get; set; }

        [JsonProperty("second")]
        public ApiGroupsItem[] Second { get; set; }

        [JsonProperty("third")]
        public ApiGroupsItem[] Third { get; set; }
    }
}