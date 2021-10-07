using System;

namespace KinectTvV2.API.Models
{
    public class TvConfig
    {
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string DisplayMessage { get; set; }
    }
}