using System;

namespace ITTV.WPF.Core.Exceptions
{
    public class MireaApiNewsException : Exception
    {
        public MireaApiNewsException() 
            : base("Не удалось получить список новостей")
        { }
    }
}