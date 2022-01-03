namespace ITTV.WPF.DataModel
{
    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {
        protected static readonly T instance = new T();
        private static object Padlock { get; } = new object();

        public static T Instance
        {
            get
            {
                lock(Padlock)
                {
                    return instance;
                }
            }
        }
    }
}
