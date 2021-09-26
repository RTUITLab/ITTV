using System.Threading.Tasks;
using KinectTvV2.Core.Services.ApiClient.Requests.GetGroups;
using KinectTvV2.Core.Services.ApiClient.Requests.GetNews;
using KinectTvV2.Core.Services.ApiClient.Requests.GetScheduleForGroup;

namespace KinectTvV2.Core.Services.ApiClient
{
    public interface IMireaApiClient
    {
        /// <summary>
        /// Получение списка новостей с указанием количества
        /// </summary>
        /// <param name="countNews"></param>
        /// <returns></returns>
        Task<ApiNewsItem[]> GetNews(int countNews = 10);
        /// <summary>
        /// Получение расписания группы
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<ApiScheduleResponse> GetScheduleForGroup(string groupName);
        /// <summary>
        /// Получение списка поддерживаемых групп
        /// </summary>
        /// <returns></returns>
        Task<ApiGroups> GetAllGroups();
    }
}
