using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {
        private static T instance = new T();
        private static readonly object padlock = new object();

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
