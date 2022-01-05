using System.Collections.Generic;
using System.IO;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Interface.Pages;
using ITTV.WPF.Network;
using Newtonsoft.Json;

namespace ITTV.WPF.DataModel
{
    public class CreateData : Singleton<CreateData>
    {
        public void GetAllTimetable()
        {
            //TODO: Cache all timetable
        }

        public void GetAllVideos()
        {
            const string uniqueId = "Video";
            
            var fullPath = AllPaths.GetDirectoryVideosPath;

            var allFiles = Directory.GetFiles(fullPath);

            var videoGroup = new DataCollection<object>(
                uniqueId,
                "Видео",
                DataCollection<object>.GroupType.Video);

            var i = 0;
            foreach (var video in allFiles)
            {
                videoGroup.Items.Add(new Video(
                    $"{uniqueId}-" + i,
                    Path.GetFileNameWithoutExtension(video),
                    typeof(VideoPage),
                    DataSource.StringToArr(video)));
                i++;
            }

            DataSource.Instance.AddToGroups(videoGroup);
        }        

        public void GetNewsFromFile()
        {
            var newsGroup = new DataCollection<object>(
                    "News",
                    "Новости",
                    DataCollection<object>.GroupType.News);
            
            NewsFromSite.Instance.SyncNewsFromSite();

            var json = File.ReadAllText(AllPaths.FileNewsCachePath);
            var newsList = JsonConvert.DeserializeObject<List<News>>(json);
            //TODO: refactoring, possible null ref exception
            foreach (var news in newsList)
            {
                newsGroup.Items.Add(news);
            }

            DataSource.Instance.AddToGroups(newsGroup);
        }

        public void GetGames()
        {
            var fullPath = AllPaths.GetDirectoryGamesPath;

            var allDir = Directory.GetDirectories(fullPath);
            
            var gamesGroup = new DataCollection<object>(
                    "Games",
                    "Игры",
                    DataCollection<object>.GroupType.Games);

            var i = 0;
            foreach (var game in allDir)
            {
                var filePath = "";
                foreach (var file in Directory.GetFiles(game))
                {
                    if (Path.GetExtension(file) == ".exe")
                    {
                        filePath = file;
                    }
                }

                gamesGroup.Items.Add(new Game(
                    "Game-" + i,
                    new DirectoryInfo(game).Name,
                    DataSource.StringToArr(filePath)));
                i++;
            }

            DataSource.Instance.AddToGroups(gamesGroup);
        }
    }
}
