using System;
using System.Timers;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Network;

namespace ITTV.WPF.DataModel
{
    public class NewsUpdateThread : IDisposable
    {
        private Timer timer;
        
        private readonly Settings _settings;
        private readonly NewsFromSite _newsFromSite;

        public NewsUpdateThread(Settings settings, NewsFromSite newsFromSite)
        {
            _settings = settings;
            _newsFromSite = newsFromSite;
        }

        public void StartUpdating()
        {
            timer = new Timer(_settings.MinForUpdate * 60 * 1000);

            timer.Elapsed += (o, e) => _newsFromSite.SyncNewsFromSite();
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
