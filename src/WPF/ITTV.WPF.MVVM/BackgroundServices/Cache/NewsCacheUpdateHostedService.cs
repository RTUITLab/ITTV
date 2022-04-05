﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.MVVM.BackgroundServices.Tracking;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.MVVM.BackgroundServices.Cache
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

            _timer = new((_) => DoWork(), null, TimeSpan.Zero, newsUpdateTime);
            
            return Task.CompletedTask;
        }
        
        private async void DoWork()
        {
            try
            {
                _logger.LogInformation("Starting sync news");

                await _mireaApiProvider.GetNews(expireDateTime: TimeSpan.Zero);
                
                _logger.LogInformation("Sync news completed");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying sync news");
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