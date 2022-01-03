using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public sealed class ApiScheduleLesson
    {
        [JsonProperty("lesson")]
        public ApiScheduleDetailLesson DetailLesson { get; set; }

        [JsonProperty("time")]
        public ApiScheduleTime Time { get; set; }
    }
}