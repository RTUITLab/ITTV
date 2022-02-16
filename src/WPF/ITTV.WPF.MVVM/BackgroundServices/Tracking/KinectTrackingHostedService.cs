using System;
using System.Threading;
using System.Threading.Tasks;
using ITTV.WPF.MVVM.Utilities.Tracking;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ITTV.WPF.MVVM.BackgroundServices.Tracking
{
    public class KinectTrackingHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<KinectTrackingHostedService> _logger;
        private readonly KinectTrackingUtility _kinectTrackingUtility;
        
        private Timer _timer = null!;

        public KinectTrackingHostedService(KinectTrackingUtility kinectTrackingUtility,
            ILogger<KinectTrackingHostedService> logger)
        {
            _kinectTrackingUtility = kinectTrackingUtility;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var handsIntervalUpdate = TimeSpan.FromMilliseconds(100);
            
            _logger.LogInformation("{0} service running", nameof(KinectTrackingHostedService));

            _timer = new Timer((_) => DoWork(), null, TimeSpan.Zero, handsIntervalUpdate);
            
            return Task.CompletedTask;
        }
        
        private void DoWork()
        {
            try
            {
                _kinectTrackingUtility.HandsStatusUpdate();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying update hands status using kinect tracking utility");
            }
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{0} is stopping",
                nameof(KinectTrackingHostedService));

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}