using HtmlAgilityPack;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using Microsoft.Samples.Kinect.ControlsBasics.Pages;

namespace Microsoft.Samples.Kinect.ControlsBasics.Network.NewsTasks
{
    public class NewsFromSite : Singleton<NewsFromSite>
    {
        public void GetNewsFromSite()
        {
            try
            {
                string URI = "https://www.mirea.ru/news/";
                List<News> news_list = new List<News>();

                HtmlWeb web = new HtmlWeb();
                var AllNewsSite = web.Load(URI);
                var NewsList = AllNewsSite.DocumentNode.SelectNodes("//div[@class='uk-card uk-card-default']");

                int i = 0;
                foreach (var news in NewsList.Take(10))
                {
                    if (news.Name == "div")
                    {
                        HtmlWeb web1 = new HtmlWeb();
                        var NewsSite = web.Load("https://www.mirea.ru/" + news.SelectSingleNode(".//a").Attributes["href"].Value);
                        var HtmlImagesList = NewsSite.DocumentNode.SelectNodes("//a[@data-fancybox='gallery']");

                        string name = NewsSite.DocumentNode.SelectSingleNode("//h1").InnerText;
                        string content = NewsSite.DocumentNode.SelectSingleNode("//div[@class='news-item-text uk-margin-bottom']").InnerText.Trim().Replace("&nbsp;", "");

                        List<byte[]> images = new List<byte[]>();
                        foreach (var HtmlImage in HtmlImagesList)
                        {
                            if (HtmlImage.Name == "a")
                            {
                                string ImagePath = "https://www.mirea.ru/" + HtmlImage.Attributes["href"].Value;
                                WebClient webClient = new WebClient();
                                byte[] data = webClient.DownloadData(ImagePath);
                                images.Add(data);
                            }
                        }

                        var temp = new News("News-" + i.ToString(), name, typeof(NewsPage), content, images);
                        news_list.Add(temp);
                        i++;
                    }

                }
                string json = JsonConvert.SerializeObject(news_list);
                File.WriteAllText("Settings/news.json", json);
            }
            catch (Exception) { MainWindow.Log("Нет доступа к сайту"); }
        }
    }
}
