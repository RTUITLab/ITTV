using System;
using System.Threading;
using System.Threading.Tasks;
using ITTV.WPF.BackgroundServices.Tracking;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Providers.MireaApi;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.BackgroundServices.Cache
{
    public class NewsCacheUpdateHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<NewsCacheUpdateHostedService> _logger;
        private readonly MireaApiProvider _mireaApiProvider;
        private readonly Settings _settings;
        
        private Timer _timer = null!;

        public NewsCacheUpdateHostedService(MireaApiProvider mireaApiProvider,
            ILogger<NewsCacheUpdateHostedService> logger,
            IOptions<Settings> settings)
        {
            _mireaApiProvider = mireaApiProvider;
            _settings = settings.Value;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var newsUpdateTime = _settings.CacheUpdateInterval;
            
            _logger.LogInformation("{0} service running", nameof(NewsCacheUpdateHostedService));

            _timer = new Timer(_ => DoWork(), null, TimeSpan.Zero, newsUpdateTime);
            
            return Task.CompletedTask;
        }
        
        private async void DoWork()
        {
            try
            {
                _logger.LogInformation("{0}: Starting sync news", 
                    nameof(NewsCacheUpdateHostedService));

                await _mireaApiProvider.GetNews(expireDateTime: TimeSpan.Zero);
                
                _logger.LogInformation("{0}: Sync news completed",
                    nameof(NewsCacheUpdateHostedService));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{0}: Error while trying sync news", 
                    nameof(NewsCacheUpdateHostedService));
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