using System;
using System.Threading.Tasks;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Providers.LocalCache;
using ITTV.WPF.Core.Services.ApiClient;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetNews;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup;

namespace ITTV.WPF.Core.Providers.MireaApi
{
    public class MireaApiProvider
    {
        private readonly TimeSpan _timeForUpdate = TimeSpan.FromHours(1);
        private readonly IMireaApiClient _mireaApiClient;
        
        public MireaApiProvider(IMireaApiClient mireaApiClient)
        {
            _mireaApiClient = mireaApiClient;
        }

        public async Task<ApiNewsItem[]> GetNews(TimeSpan? expireDateTime = default)
        {
            var timeUpdated = expireDateTime ?? _timeForUpdate;
            
            Func<Task<ApiNewsItem[]>> dataSource = () => _mireaApiClient.GetNews();

            var news = await LocalCacheProvider.GetAsync(LocalCacheHelper.NewsCacheKey, dataSource, timeUpdated);
            return news;
        }
        public async Task<ApiGroups> GetGroups()
        {
            var timeUpdated = _timeForUpdate;

            Func<Task<ApiGroups>> dataSource = () => _mireaApiClient.GetAllGroups();
            
            var groups = await LocalCacheProvider.GetAsync(LocalCacheHelper.GroupsCacheKey, dataSource, timeUpdated);
            return groups;
        }
        public async Task<ApiFullScheduleResponse> GetFullSchedule(string group)
        {
            var timeUpdated = _timeForUpdate;
            
            Func<Task<ApiFullScheduleResponse>> dataSource = () => _mireaApiClient.GetFullScheduleForGroup(group);
            
            var schedule = await LocalCacheProvider.GetAsync(LocalCacheHelper.GroupScheduleCacheKey(group), dataSource, timeUpdated);
            return schedule;
        }
    }
}