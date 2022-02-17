using System;

namespace ITTV.WPF.Core.Exceptions
{
    public class MireaApiScheduleGroupsException : Exception
    {
        public MireaApiScheduleGroupsException()
            : base("Не удалось получить список групп ИИТ")
        { }
    }
}