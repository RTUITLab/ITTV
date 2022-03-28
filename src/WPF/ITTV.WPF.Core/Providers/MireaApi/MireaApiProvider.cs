using System;
using System.Threading.Tasks;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Providers.LocalCache;
using ITTV.WPF.Core.Services.ApiClient;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetNews;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.Core.Providers.MireaApi
{
    public class MireaApiProvider
    {
        private readonly IMireaApiClient _mireaApiClient;
        private readonly Settings _settings;
        
        public MireaApiProvider(IMireaApiClient mireaApiClient,
                IOptions<Settings> settings)
        {
            _mireaApiClient = mireaApiClient;
            _settings = settings.Value;
        }

        public async Task<ApiNewsItem[]> GetNews(TimeSpan? expireDateTime = default)
        {
            var timeUpdated = expireDateTime ?? _settings.CacheUpdateInterval;
            
            Func<Task<ApiNewsItem[]>> dataSource = () => _mireaApiClient.GetNews();

            var news = await LocalCacheProvider.GetAsync(LocalCacheHelper.NewsCacheKey, dataSource, timeUpdated);
            return news;
        }
        public async Task<ApiGroups> GetGroups()
        {
            var timeUpdated = _settings.CacheUpdateInterval;

            Func<Task<ApiGroups>> dataSource = () => _mireaApiClient.GetAllGroups();
            
            var groups = await LocalCacheProvider.GetAsync(LocalCacheHelper.GroupsCacheKey, dataSource, timeUpdated);
            return groups;
        }
        public async Task<ApiFullScheduleResponse> GetFullSchedule(string group)
        {
            var timeUpdated = _settings.CacheUpdateInterval;
            
            Func<Task<ApiFullScheduleResponse>> dataSource = () => _mireaApiClient.GetFullScheduleForGroup(group);
            
            var schedule = await LocalCacheProvider.GetAsync(LocalCacheHelper.GroupScheduleCacheKey(group), dataSource, timeUpdated);
            return schedule;
        }
    }
}