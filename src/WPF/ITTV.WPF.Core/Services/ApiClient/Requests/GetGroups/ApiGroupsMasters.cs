using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups
{
    public class ApiGroupsMasters
    {
        [JsonProperty("first")]
        public ApiGroupsItem[] First { get; set; }

        [JsonProperty("second")]
        public ApiGroupsItem[] Second { get; set; }
    }
}