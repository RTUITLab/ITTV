using Newtonsoft.Json;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup
{
    public class ApiScheduleForDay
    {
        [JsonProperty("lessons")] 
        public ApiLesson[][] Weeks { get; set; }
    }
}