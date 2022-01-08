using System.IO;

namespace ITTV.WPF.MVVM
{
    public static class AllPaths
    {
        private const string DirectoryVideosPath = @"Videos\";
        public static string GetDirectoryVideosPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryVideosPath);
                return DirectoryVideosPath;
            }
        }
        
        private const string DirectoryBackgroundVideosPath = @"Videos\Background\";
        public static string GetDirectoryBackgroundVideosPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryBackgroundVideosPath);
                return DirectoryBackgroundVideosPath;
            }
        }
        
        private const string DirectoryGamesPath = @"Games\";
        public static string GetDirectoryGamesPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryGamesPath);
                return DirectoryGamesPath;
            }
        }
        
        private const string DirectoryCachePath = @"Cache\";
        public static string GetDirectoryCachePath
        {
            get
            {
                Directory.CreateDirectory(DirectoryCachePath);
                return DirectoryCachePath;
            }
        }
        
        private const string DirectoryTimeTablesPath = @"TimeTables\";
        public static string GetDirectoryTimeTablesPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryTimeTablesPath);
                return DirectoryTimeTablesPath;
            }
        }

        private const string DirectoryGestureDatabasePath = @"Gesture\";
        public static string GetDirectoryGestureDatabasePath
        {
            get
            {
                Directory.CreateDirectory(DirectoryGestureDatabasePath);
                return DirectoryGestureDatabasePath;
            }
        }

        private const string DirectoryEggPath = @"vgbtechs\";
        public static string GetDirectoryEggPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryEggPath);
                return DirectoryEggPath;
            }
        }

        public const string FileSettingsPath = "settings.json";
        public const string FileLogsPath = "./logs.txt";

        public static readonly string FileGroupsCachePath = GetDirectoryCachePath + "groups.json";
        public static readonly string FileNewsCachePath = GetDirectoryCachePath + "news.json";

        public static readonly string FileGestureDatabasePath = GetDirectoryGestureDatabasePath + "KinectGesture.gbd";

        public static readonly string FileEggVideoPath = GetDirectoryEggPath + "kinectrequired.mp4";
    }
}