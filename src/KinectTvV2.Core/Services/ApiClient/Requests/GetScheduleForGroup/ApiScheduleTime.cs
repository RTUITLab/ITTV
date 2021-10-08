using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public sealed class ApiScheduleTime
    {
        [JsonProperty("end")]
        public string End { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }
    }
}