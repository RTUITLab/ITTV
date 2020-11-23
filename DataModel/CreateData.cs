using System;
using System.IO;
using Microsoft.Samples.Kinect.ControlsBasics.Pages;
using HtmlAgilityPack;

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

            SampleDataCollection course_group = new SampleDataCollection(
                "Courses",
                "Расписание",
                SampleDataCollection.GroupType.Courses);

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
                course_group.Items.Add(new SampleDataItem(
                "Course-" + i.ToString(),
                name,
                SampleDataItem.TaskType.Page,
                typeof(ScrollViewerSample),
                SampleDataSource.StringToArr(image)));

                i++;
            }

            SampleDataSource.AddToGroups(course_group);

        }

        public static void GetAllVideos()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Videos\";


            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string[] AllFiles = Directory.GetFiles(fullPath);

            SampleDataCollection video_group = new SampleDataCollection(
                "Video",
                "Видео",
                SampleDataCollection.GroupType.Video);

            int i = 0;
            foreach (var video in AllFiles)
            {
                video_group.Items.Add(new SampleDataItem(
                    "Video-" + i.ToString(),
                    Path.GetFileNameWithoutExtension(video),
                    string.Empty,
                    SampleDataItem.TaskType.Page,
                    typeof(VideoPage),
                    SampleDataSource.StringToArr(video)));

                i++;
            }

            SampleDataSource.AddToGroups(video_group);
        }

        public static void GetBackgroundVideos()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Videos\Background\";


            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string[] AllFiles = Directory.GetFiles(fullPath);

            SampleDataCollection video_group = new SampleDataCollection(
                "Video-Background",
                "Видео на фоне",
                SampleDataCollection.GroupType.Video);

            int i = 0;
            foreach (var video in AllFiles)
            {
                video_group.Items.Add(new SampleDataItem(
                   "Video-" + i.ToString(),
                   Path.GetFileNameWithoutExtension(video),
                   string.Empty,
                   SampleDataItem.TaskType.Page,
                   typeof(VideoPage),
                   SampleDataSource.StringToArr(video)));

                i++;
            }

            SampleDataSource.AddToGroups(video_group);
        }

        public static void GetNewsFromSite()
        {
            try
            {
                string URI = "https://www.mirea.ru/news/";

                SampleDataCollection news_group = new SampleDataCollection(
                    "News",
                    "Новости",
                    SampleDataCollection.GroupType.News);

                HtmlWeb web = new HtmlWeb();
                var HtmlDoc = web.Load(URI);
                var NewsList = HtmlDoc.DocumentNode.SelectSingleNode("//div[@id='page-wrapper']/div[2]/div/div[2]/div/div[2]/div[1]/div[1]").ChildNodes;

                int i = 0;
                string name;
                string source;
                string news_page;
                foreach (var News in NewsList)
                {
                    if (News.Name == "div")
                    {
                        name = News.SelectSingleNode(".//a[@class='uk-link-reset']").InnerText.Trim();
                        source = "https://www.mirea.ru/" + News.SelectSingleNode(".//img[@class='uk-transition-scale-up uk-transition-opaque']").Attributes["src"].Value;
                        news_page = "https://www.mirea.ru/" + News.SelectSingleNode(".//a[@class='uk-link-reset']").Attributes["href"].Value;

                        news_group.Items.Add(new SampleDataItem(
                            "News-" + i.ToString(),
                            name,
                            source,
                            SampleDataItem.TaskType.Page,
                            typeof(News),
                            news_page));
                        i++;
                    }

                }

                SampleDataSource.AddToGroups(news_group);
            }
            catch (Exception) { MainWindow.Log("Нет доступа к сайту"); }
        }

        public static void GetGames()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Games\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string[] AllDir = Directory.GetDirectories(fullPath);


            SampleDataCollection games_group = new SampleDataCollection(
                    "Games",
                    "Игры",
                    SampleDataCollection.GroupType.Video);

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

                games_group.Items.Add(new SampleDataItem(
                    "Game-" + i.ToString(),
                    new DirectoryInfo(Game).Name,
                    string.Empty,
                    SampleDataItem.TaskType.Execute,
                    typeof(VideoPage),
                    SampleDataSource.StringToArr(filePath)));
                i++;
            }

            SampleDataSource.AddToGroups(games_group);
        }
    }
}
