using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    class NewsUpdateThread
    {
        private static Timer timer;

        public static void StartUpdating()
        {
            timer = new Timer((e) =>
            {
                CreateData.GetNewsFromSite();
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(Settings.Settings.MinForUpdate));
        }

        public static void StopUpdating()
        {
            timer.Dispose();
            timer = null;
        }
    }
}
