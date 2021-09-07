using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var result = new List<ApiNewsItem>();
            
            const string cardClassName = "uk-card-media-top";
            var query = $".{cardClassName} a";
            
            var document = new HtmlParser().ParseDocument(contentHtml);
            var cards = document.QuerySelectorAll(query)
                .Take(countNews);

            foreach (var card in cards)
            {
                var link = card.Attributes["href"]?.Value;
                
                var responseByCard = await _httpClient.GetAsync(link);
                var responseByCardHtml = await responseByCard.Content.ReadAsStringAsync();

                var cardDocument = new HtmlParser().ParseDocument(responseByCardHtml);

                var cardDetail = cardDocument.QuerySelector(".uk-grid-margin");
            }
            
            return Array.Empty<ApiNewsItem>();
        }
        
    }
}
