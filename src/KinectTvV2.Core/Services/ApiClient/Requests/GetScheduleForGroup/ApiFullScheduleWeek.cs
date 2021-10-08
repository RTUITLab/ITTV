using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public class ApiFullScheduleWeek
    {
        [JsonProperty("monday")]
        public ApiScheduleLesson[] Monday { get; set; }
        
        [JsonProperty("tuesday")]
        public ApiScheduleLesson[] Tuesday { get; set; }
        
        [JsonProperty("wednesday")]
        public ApiScheduleLesson[] Wednesday { get; set; }
        
        [JsonProperty("thursday")]
        public ApiScheduleLesson[] Thursday { get; set; }
        
        [JsonProperty("friday")]
        public ApiScheduleLesson[] Friday { get; set; }
        
        [JsonProperty("saturday")]
        public ApiScheduleLesson[] Saturday { get; set; }
    }
}