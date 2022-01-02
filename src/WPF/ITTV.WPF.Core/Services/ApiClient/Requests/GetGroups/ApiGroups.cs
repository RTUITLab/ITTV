using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups
{
    public class ApiGroups
    {
        [JsonProperty("bachelor")]
        public ApiGroupsBachelors Bachelor { get; set; }

        [JsonProperty("master")]
        public ApiGroupsMasters Master { get; set; }
    }
}