using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public sealed class ApiScheduleDetailLesson
    {
        [JsonProperty("classRoom")]
        public string ClassRoom { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("teacher")]
        public string Teacher { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}