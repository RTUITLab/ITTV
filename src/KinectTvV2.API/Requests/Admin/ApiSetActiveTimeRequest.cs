using System;
using System.Text.Json.Serialization;

namespace KinectTvV2.API.Requests.Admin
{
    public sealed class ApiSetActiveTimeRequest
    {
        public ApiSetActiveTimeRequest()
        { }

        public ApiSetActiveTimeRequest(TimeRequest timeFrom, TimeRequest timeTo)
        {
            TimeFrom = timeFrom;
            TimeTo = timeTo;
        }
        public TimeRequest TimeFrom { get; set; }
        public TimeRequest TimeTo { get; set; }
    }
    [Serializable]
    public class TimeRequest
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        [JsonIgnore]
        public TimeSpan GetTimeSpan => new(0, Hours, Minutes, 0);
    }
}