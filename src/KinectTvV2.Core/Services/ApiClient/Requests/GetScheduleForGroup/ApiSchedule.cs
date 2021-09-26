using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public class ApiSchedule
    {
        [JsonProperty("1")]
        public ApiScheduleForDay Monday { get; set; }

        [JsonProperty("2")]
        public ApiScheduleForDay Tuesday { get; set; }

        [JsonProperty("3")]
        public ApiScheduleForDay Wednesday { get; set; }

        [JsonProperty("4")]
        public ApiScheduleForDay Thursday { get; set; }

        [JsonProperty("5")]
        public ApiScheduleForDay Friday { get; set; }

        [JsonProperty("6")]
        public ApiScheduleForDay Saturday { get; set; }
    }
}