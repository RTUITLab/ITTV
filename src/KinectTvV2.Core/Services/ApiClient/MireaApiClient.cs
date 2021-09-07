using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using KinectTvV2.Core.Services.ApiClient.Requests.GetNews;

namespace KinectTvV2.Core.Services.ApiClient
{
    public class MireaApiClient : IMireaApiClient
    {
        private readonly HttpClient _httpClient;

        public MireaApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(MireaApiEndpoints.BaseAddress)
            };
        }

        public async Task<ApiNewsItem[]> GetNews(int countNews = 10)
        {
            var response = await _httpClient.GetAsync(MireaApiEndpoints.GetNewsEndpoint);
            var responseMessage = await response.Content.ReadAsStringAsync();

            var result = await ParseToNews(responseMessage, countNews);
            return result;
        }

        public Task GetScheduler(string groupName = null)
        {
            throw new System.NotImplementedException();
        }

        private async Task<ApiNewsItem[]> ParseToNews(string contentHtml, int countNews)
        {
            const string cardClassName = "uk-card-media-top";
            var query = $".{cardClassName} a";
            
            var document = new HtmlParser().ParseDocument(contentHtml);
            var cards = document.QuerySelectorAll(query)
                .Take(countNews);

            var semaphore = new SemaphoreSlim(5);

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
            
            var linkCard = card.Attributes["href"]?.Value;

            var responseByCard = await _httpClient.GetAsync(linkCard);
            var responseByCardHtml = await responseByCard.Content.ReadAsStringAsync();

            var cardDocument = new HtmlParser().ParseDocument(responseByCardHtml);
            var cardDetailNewsElement = cardDocument.GetElementsByClassName(cardNewsClassName)
                .FirstOrDefault();

            var cardNewsText = cardDetailNewsElement?.TextContent;

            var result = new ApiNewsItem(cardTitleText, cardNewsText, linkCard);
            return result;
        }
    }
}
