using Microsoft.Samples.Kinect.ControlsBasics.Network.NewsTasks;
using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Timers;
using Microsoft.Samples.Kinect.ControlsBasics.Network;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    class NewsUpdateThread : Singleton<NewsUpdateThread>, IDisposable
    {
        private Timer timer;

        public void StartUpdating()
        {
            timer = new Timer(Settings.instance.MinForUpdate * 60 * 1000);

            timer.Elapsed += (o, e) => NewsFromSite.Instance.SyncNewsFromSite();
            timer.Elapsed += async (o,e ) => await new TimeTableNetwork().SyncGroupsToFile();
            
            timer.Start();
        }

        public void StopUpdating()
        {
            timer.Stop();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
