using System;
using System.Timers;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Network;

namespace ITTV.WPF.DataModel
{
    public class NewsUpdateThread : Singleton<NewsUpdateThread>, IDisposable
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
