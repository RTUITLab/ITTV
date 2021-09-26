using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public class ApiScheduleResponse
    {
        [JsonProperty("schedule")]
        public ApiSchedule Result { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
    }
}