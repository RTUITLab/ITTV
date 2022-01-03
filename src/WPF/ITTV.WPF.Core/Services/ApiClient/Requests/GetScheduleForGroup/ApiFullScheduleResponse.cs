using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public sealed class ApiFullScheduleResponse
    {
        [JsonProperty("first")]
        public ApiFullScheduleWeek FirstWeek { get; set; }

        [JsonProperty("second")]
        public ApiFullScheduleWeek SecondWeek { get; set; }
    }
}