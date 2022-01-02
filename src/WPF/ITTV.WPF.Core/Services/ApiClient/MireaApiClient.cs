using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetGroups;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetNews;
using ITTV.WPF.Core.Services.ApiClient.Requests.GetScheduleForGroup;
using Newtonsoft.Json;

namespace ITTV.WPF.Core.Services.ApiClient
{
    public class MireaApiClient : IMireaApiClient
    {
        private readonly HttpClient _httpClient;

        public MireaApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ApiNewsItem[]> GetNews(int countNews = 10)
        {
            var uri = MireaApiEndpoints.GetNewsEndpoint;
            var response = await _httpClient.GetAsync(uri);
            var responseMessage = await response.Content.ReadAsStringAsync();

            var result = await ParseToNews(responseMessage, countNews);
            return result;
        }

        public async Task<ApiFullSheduleResponse> GetFullScheduleForGroup(string groupName)
        {
            var response = await _httpClient.GetAsync(MireaApiEndpoints.GetFullScheduleForGroup(groupName));
            var responseMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiFullSheduleResponse>(responseMessage);
            return result;
        }

        public async Task<ApiScheduleLesson[]> GetTodayScheduleForGroup(string groupName)
        {
            var response = await _httpClient.GetAsync(MireaApiEndpoints.GetTodayScheduleForGroup(groupName));
            var responseMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiScheduleLesson[]>(responseMessage);
            return result;
        }

        public async Task<ApiScheduleLesson[]> GetTomorrowScheduleForGroup(string groupName)
        {
            var response = await _httpClient.GetAsync(MireaApiEndpoints.GetTomorrowScheduleForGroup(groupName));
            var responseMessage = await response.Content.ReadAsStringAsync();
    
            var result = JsonConvert.DeserializeObject<ApiScheduleLesson[]>(responseMessage);
            return result;
        }

        public async Task<ApiGroups> GetAllGroups()
        {
            var response = await _httpClient.GetAsync(MireaApiEndpoints.GetAllGroups);
            var responseMessage = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiGroups>(responseMessage);
            return result;
        }

        private async Task<ApiNewsItem[]> ParseToNews(string contentHtml, int countNews)
        {
            const string cardClassName = "uk-card-media-top";
            var query = $".{cardClassName} a";
            
            var document = new HtmlParser().ParseDocument(contentHtml);
            var cards = document.QuerySelectorAll(query)
                .Take(countNews);

            const int countThreads = 5;
            using var semaphore = new SemaphoreSlim(countThreads);

            var result = new List<ApiNewsItem>();

            var tasks = cards.Select(async x =>
            {
                try
                {
                    await semaphore.WaitAsync();
                    var newItem = await GetDetailsByCard(x);
                    result.Add(newItem);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
            
            return result.ToArray();
        }

        private async Task<ApiNewsItem> GetDetailsByCard(IElement card)
        {
            const string cardTitleClassName = "enableSrcset";
            const string cardNewsClassName = "news-item-text";

            var cardTitleElement = card.GetElementsByClassName(cardTitleClassName)
                .FirstOrDefault() as IHtmlElement;
            var cardTitleText = cardTitleElement?.Title;

            
            var linkCard = MireaApiEndpoints.NewsBaseAddress + card.Attributes["href"]?.Value;

            var responseByCard = await _httpClient.GetAsync(linkCard);
            var responseByCardHtml = await responseByCard.Content.ReadAsStringAsync();

            var cardDocument = new HtmlParser().ParseDocument(responseByCardHtml);
            var cardDetailNewsElement = cardDocument.GetElementsByClassName(cardNewsClassName)
                .FirstOrDefault();

            var cardNewsText = cardDetailNewsElement?.TextContent;

            var photos = await GetPhotos(cardDocument);

            var result = new ApiNewsItem(cardTitleText, cardNewsText, linkCard, photos);
            return result;
        }

        private async Task<ApiImageItem[]> GetPhotos(IParentNode detailCardDocument)
        {
            var result = new List<ApiImageItem>();
            
            const string photoQuery = "[data-fancybox=gallery]";

            var cardPhotosElements = detailCardDocument.QuerySelectorAll(photoQuery);

            foreach (var cardPhotoElement in cardPhotosElements)
            {
                var link = MireaApiEndpoints.NewsBaseAddress + cardPhotoElement.Attributes["href"]?.Value;
               
                var name = link?.Split('/').Last();
                var photoData = await _httpClient.GetByteArrayAsync(link);

                var newFile = new ApiImageItem(link, photoData, name);
                result.Add(newFile);
            }
            
            return result.ToArray();
        }
    }
}
