using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public sealed class ApiFullSheduleResponse
    {
        [JsonProperty("first")]
        public ApiFullScheduleWeek FirstWeek { get; set; }

        [JsonProperty("second")]
        public ApiFullScheduleWeek SecondWeek { get; set; }
    }
}