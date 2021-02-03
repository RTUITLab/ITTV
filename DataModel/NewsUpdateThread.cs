using Microsoft.Samples.Kinect.ControlsBasics.Network.NewsTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    class NewsUpdateThread : Singleton<NewsUpdateThread>
    {
        private Timer timer;

        public void StartUpdating()
        {
            timer = new Timer((e) =>
            {
                NewsFromSite.Instance.GetNewsFromSite();
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(Settings.Settings.MinForUpdate));
        }

        public void StopUpdating()
        {
            timer.Dispose();
            timer = null;
        }
    }
}
