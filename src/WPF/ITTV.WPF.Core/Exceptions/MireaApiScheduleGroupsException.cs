using System;

namespace ITTV.WPF.Core.Exceptions
{
    public class MireaApiScheduleGroupsException : Exception
    {
        public MireaApiScheduleGroupsException(string statusCode)
            : base($"Не удалось получить список групп ИИТ, status code:{statusCode}")
        { }
    }
}