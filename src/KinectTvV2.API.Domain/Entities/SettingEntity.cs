using System;

namespace KinectTvV2.API.Domain.Entities
{
    public class SettingEntity : Entity
    {
        private SettingEntity()
        { }

        public SettingEntity(TimeSpan timeFrom,
            TimeSpan timeTo,
            string displayMessage)
        {
            TimeFrom = timeFrom;
            TimeTo = timeTo;
            DisplayMessage = displayMessage;
        }
        public TimeSpan TimeFrom { get; private set; }
        public TimeSpan TimeTo { get; private set; }
        public string DisplayMessage { get; private set; }


        public void UpdateDisplayMessage(string displayMessage)
        {
            DisplayMessage = displayMessage;
            SetUpdated();
        }

        public void SetDateFrom(TimeSpan timeFrom)
        {
            TimeFrom = timeFrom;
            SetUpdated(); 
        }

        public void SetDateTo(TimeSpan timeTo)
        {
            TimeTo = timeTo;
            SetUpdated();
        }
    }
}