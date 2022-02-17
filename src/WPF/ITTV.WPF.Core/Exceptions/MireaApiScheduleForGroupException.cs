using System;

namespace ITTV.WPF.Core.Exceptions
{
    public class MireaApiScheduleForGroupException : Exception
    {
        public MireaApiScheduleForGroupException(string groupName)
            : base($"Не удалось получить расписание для группы {groupName}")
        { }
    }
}