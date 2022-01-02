using System.Threading.Tasks;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetNews;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup;

namespace ITTV.WPF.Core.Services.ApiClient
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
        /// Получение полного расписания группы
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<ApiFullSheduleResponse> GetFullScheduleForGroup(string groupName);
        /// <summary>
        /// Получение расписания группы на сегодня
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<ApiScheduleLesson[]> GetTodayScheduleForGroup(string groupName);
        /// <summary>
        /// Получение расписания группы на завтра
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<ApiScheduleLesson[]> GetTomorrowScheduleForGroup(string groupName);
        /// <summary>
        /// Получение списка поддерживаемых групп
        /// </summary>
        /// <returns></returns>
        Task<ApiGroups> GetAllGroups();
    }
}
