using System;

namespace KinectTvV2.API.Requests.Admin
{
    public sealed class ApiSetActiveTimeRequest
    {
        public ApiSetActiveTimeRequest()
        { }

        public ApiSetActiveTimeRequest(DateTime dateFrom, DateTime dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}