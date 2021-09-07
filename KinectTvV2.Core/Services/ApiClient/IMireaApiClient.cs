using System.Threading.Tasks;
using KinectTvV2.Core.Services.ApiClient.Requests.GetNews;

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
        /// Получение расписания группы, опциональный фильтр по названию группы 
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task GetScheduler(string groupName = null);
    }
}
