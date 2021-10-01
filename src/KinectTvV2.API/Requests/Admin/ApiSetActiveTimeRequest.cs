using System;

namespace KinectTvV2.API.Requests.Admin
{
    public sealed class ApiSetActiveTimeRequest
    {
        public ApiSetActiveTimeRequest()
        { }

        public ApiSetActiveTimeRequest(TimeSpan timeFrom, TimeSpan timeTo)
        {
            TimeFrom = timeFrom;
            TimeTo = timeTo;
        }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
    }
}