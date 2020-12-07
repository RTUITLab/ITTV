using System;
using System.IO;
using Microsoft.Samples.Kinect.ControlsBasics.Pages;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using HtmlAgilityPack;
using static Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models.DataBase;
using static Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models.DataSource;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    public class CreateData
    {
        public static void GetAllTimetable()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"TimeTables\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            Console.WriteLine(fullPath);
            string[] AllFiles = Directory.GetFiles(fullPath);

            DataCollection<object> course_group = new DataCollection<object>(
                "Courses",
                "Расписание",
                DataCollection<object>.GroupType.Courses);

            int i = 0;
            string name;
            foreach (var image in AllFiles)
            {
                switch (Path.GetFileNameWithoutExtension(image))
                {
                    case "1":
                        name = "Первый курс";
                        break;
                    case "2":
                        name = "Второй курс";
                        break;
                    case "3":
                        name = "Третий курс";
                        break;
                    case "4":
                        name = "Четвертый курс";
                        break;
                    case "5":
                        name = "Первый курс магистратуры";
                        break;
                    case "6":
                        name = "Второй курс магистратуры";
                        break;
                    default:
                        name = "";
                        break;
                }
                course_group.Items.Add(new Page(
                "Course-" + i.ToString(),
                name,
                typeof(ScrollViewerSample),
                StringToArr(image)));

                i++;
            }

            AddToGroups(course_group);

        }

        public static void GetAllVideos()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Videos\";


            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string[] AllFiles = Directory.GetFiles(fullPath);

            DataCollection<object> video_group = new DataCollection<object>(
                "Video",
                "Видео",
                DataCollection<object>.GroupType.Video);

            int i = 0;
            foreach (var video in AllFiles)
            {
                video_group.Items.Add(new Video(
                    "Video-" + i.ToString(),
                    Path.GetFileNameWithoutExtension(video),
                    typeof(VideoPage),
                    StringToArr(video),
                    video));

                i++;
            }

            AddToGroups(video_group);
        }

        public static void GetBackgroundVideos()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Videos\Background\";


            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string[] AllFiles = Directory.GetFiles(fullPath);

            DataCollection<object> video_group = new DataCollection<object>(
                "Video-Background",
                "Видео на фоне",
                DataCollection<object>.GroupType.Video);

            int i = 0;
            foreach (var video in AllFiles)
            {
                video_group.Items.Add(new Video(
                   "Video-" + i.ToString(),
                   Path.GetFileNameWithoutExtension(video),
                   typeof(VideoPage),
                   StringToArr(video),
                   ""));

                i++;
            }

            AddToGroups(video_group);
        }

        public static void GetNewsFromSite()
        {
            try
            {
                string URI = "https://www.mirea.ru/news/";
                List<News> news_list = new List<News>();

                HtmlWeb web = new HtmlWeb();
                var HtmlDoc = web.Load(URI);
                var NewsList = HtmlDoc.DocumentNode.SelectNodes("//div[contains(@id, 'bx')]");

                int i = 0;
                foreach (var news in NewsList.Take(10))
                {
                    if (news.Name == "div")
                    {
                        HtmlWeb web1 = new HtmlWeb();
                        var HtmlDoc1 = web.Load("https://www.mirea.ru/" + news.SelectSingleNode(".//a[@class='uk-link-reset']").Attributes["href"].Value);
                        var HtmlImagesList = HtmlDoc1.DocumentNode.SelectNodes("//a[@data-fancybox='gallery']");

                        string name = HtmlDoc1.DocumentNode.SelectSingleNode("//h1").InnerText.Trim();
                        string content = HtmlDoc1.DocumentNode.SelectSingleNode("//div[@class='news-item-text uk-margin-bottom']").InnerText.Trim().Replace("&nbsp;", "");

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

        public static void GetNewsFromFile()
        {
            DataCollection<object> news_group = new DataCollection<object>(
                    "News",
                    "Новости",
                    DataCollection<object>.GroupType.News);

            string json = File.ReadAllText("Settings/news.json");
            List<News> news_list = JsonConvert.DeserializeObject<List<News>>(json);

            foreach (News news in news_list)
            {
                news_group.Items.Add(news);
            }

            AddToGroups(news_group);
        }

        public static void GetGames()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Games\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string[] AllDir = Directory.GetDirectories(fullPath);


            DataCollection<object> games_group = new DataCollection<object>(
                    "Games",
                    "Игры",
                    DataCollection<object>.GroupType.Video);

            int i = 0;
            foreach (var Game in AllDir)
            {
                string filePath = "";
                foreach (var file in Directory.GetFiles(Game))
                {
                    if (Path.GetExtension(file) == ".exe")
                    {
                        filePath = file;
                    }
                }

                games_group.Items.Add(new Game(
                    "Game-" + i.ToString(),
                    new DirectoryInfo(Game).Name,
                    StringToArr(filePath)));
                i++;
            }

            AddToGroups(games_group);
        }
    }
}
