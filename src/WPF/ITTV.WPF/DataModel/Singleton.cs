namespace ITTV.WPF.DataModel
{
    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {
        protected static readonly T instance = new T();
        private static object padlock { get; } = new object();

        public static T Instance
        {
            get
            {
                lock(padlock)
                {
                    return instance;
                }
            }
        }
    }
}
