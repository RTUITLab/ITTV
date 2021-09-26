using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public class ApiLesson
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("weeks")]
        public long[] Weeks { get; set; }

        [JsonProperty("time_start")]
        public string TimeStart { get; set; }

        [JsonProperty("time_end")]
        public string TimeEnd { get; set; }

        [JsonProperty("types")]
        public string Types { get; set; }

        [JsonProperty("teachers")]
        public string[] Teachers { get; set; }

        [JsonProperty("rooms")]
        public string[] Rooms { get; set; }
    }
}