using System;

namespace ITTV.API.Core.Models.ITTV
{
    public class ITTVConfiguration
    {
        public string DisplayMessage { get; set; }
        public TimeSpan TimeFrom { get; set; } = new(0, 8, 30, 0);
        public TimeSpan TimeTo { get; set; } = new(0, 22, 0, 0);

        public void SetDisplayMessage(string displayMessage)
        {
            DisplayMessage = displayMessage;
        }

        public void SetActiveTime(TimeSpan timeFrom, TimeSpan timeTo)
        {
            TimeFrom = timeFrom;
            TimeTo = timeTo;
        }
    }
}