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
        private readonly IMireaApiClient _mireaApiClient;
        
        public MireaApiProvider(IMireaApiClient mireaApiClient)
        {
            _mireaApiClient = mireaApiClient;
        }

        public async Task<ApiNewsItem[]> GetNews()
        {
            var timeUpdated = TimeSpan.FromMinutes(5);

            var dataSource = _mireaApiClient.GetNews();

            var news = await LocalCacheProvider.GetAsync(LocalCacheHelper.NewsCacheKey, dataSource, timeUpdated);
            return news;
        }
        public async Task<ApiGroups> GetGroups()
        {
            var timeUpdated = TimeSpan.FromMinutes(5);

            var dataSource = _mireaApiClient.GetAllGroups();

            var groups = await LocalCacheProvider.GetAsync(LocalCacheHelper.GroupsCacheKey, dataSource, timeUpdated);
            return groups;
        }
        public async Task<ApiFullScheduleResponse> GetFullSchedule(string group)
        {
            var timeUpdated = TimeSpan.FromMinutes(5);

            var dataSource = _mireaApiClient.GetFullScheduleForGroup(group);

            var schedule = await LocalCacheProvider.GetAsync(LocalCacheHelper.GroupScheduleCacheKey(group), dataSource, timeUpdated);
            return schedule;
        }
    }
}