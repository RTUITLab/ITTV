using System;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.Services
{
    public class MireaTimeManager
    {
        private readonly Settings _settings;
        
        public MireaTimeManager(Settings settings)
        {
            _settings = settings;
        }
        
        public bool IsWorkTime(TimeSpan timeSpan)
        {
            //TODO: Add startTime value
            return !_settings.NeedCheckTime || timeSpan.Hours < _settings.SleepHour && timeSpan.Hours >= 8;
        }

        public bool IsWorkTimeNow()
            => IsWorkTime(DateTime.Now.TimeOfDay);
    }
}
