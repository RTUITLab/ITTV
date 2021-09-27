using System;
using System.IO;
using Microsoft.Samples.Kinect.ControlsBasics.Pages;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using static Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models.DataSource;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Samples.Kinect.ControlsBasics.Network.NewsTasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    public class CreateData : Singleton<CreateData>
    {
        public void GetAllTimetable()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"TimeTables\";


            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public void GetAllVideos()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Videos\";


            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            string[] allFiles = Directory.GetFiles(fullPath);

            DataCollection<object> video_group = new DataCollection<object>(
                "Video",
                "Видео",
                DataCollection<object>.GroupType.Video);

            int i = 0;
            foreach (var video in allFiles)
            {
                video_group.Items.Add(new Video(
                    "Video-" + i.ToString(),
                    Path.GetFileNameWithoutExtension(video),
                    typeof(VideoPage),
                    StringToArr(video)));

                i++;
            }

            DataSource.Instance.AddToGroups(video_group);
        }        

        public void GetNewsFromFile()
        {
            DataCollection<object> newsGroup = new DataCollection<object>(
                    "News",
                    "Новости",
                    DataCollection<object>.GroupType.News);
            
            NewsFromSite.Instance.SyncNewsFromSite();

            var json = File.ReadAllText("Settings/news.json");
            var newsList = JsonConvert.DeserializeObject<List<News>>(json);
            
            foreach (var news in newsList)
            {
                newsGroup.Items.Add(news);
            }

            DataSource.Instance.AddToGroups(newsGroup);
        }

        public void GetGames()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + @"Games\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            var allDir = Directory.GetDirectories(fullPath);


            DataCollection<object> games_group = new DataCollection<object>(
                    "Games",
                    "Игры",
                    DataCollection<object>.GroupType.Games);

            int i = 0;
            foreach (var Game in allDir)
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

            DataSource.Instance.AddToGroups(games_group);
        }
    }
}
