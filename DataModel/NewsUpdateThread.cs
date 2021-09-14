using Microsoft.Samples.Kinect.ControlsBasics.Network.NewsTasks;
using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Timers;
using Microsoft.Samples.Kinect.ControlsBasics.Network;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    class NewsUpdateThread : Singleton<NewsUpdateThread>, IDisposable
    {
        private Timer _timer;

        public void StartUpdating()
        {
            _timer = new Timer(Settings.instance.MinForUpdate * 60 * 1000);

            _timer.Elapsed += (o, e) => NewsFromSite.Instance.SyncNewsFromSite();
            _timer.Elapsed += async (o,e ) => await new TimeTableNetwork().SyncGroupsToFile();
            
            _timer.Start();
        }

        public void StopUpdating()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
