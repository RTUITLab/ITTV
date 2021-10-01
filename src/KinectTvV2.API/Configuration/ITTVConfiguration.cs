using System;

namespace KinectTvV2.API.Configuration
{
    public class ITTVConfiguration
    {
        public ITTVConfiguration()
        { }

        public ITTVConfiguration(TimeSpan timeFrom, TimeSpan timeTo, string displayMessage)
        {
            TimeFrom = timeFrom;
            TimeTo = timeTo;
            DisplayMessage = displayMessage;
        }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string DisplayMessage { get; set; }
    }
}