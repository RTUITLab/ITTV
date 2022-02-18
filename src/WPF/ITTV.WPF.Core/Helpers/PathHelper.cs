using System;
using System.IO;
using ITTV.WPF.Abstractions.Enums;

namespace ITTV.WPF.Core.Helpers
{
    public static class PathHelper
    {
        private static readonly string DirectoryVideosPath = Path.Combine(Directory.GetCurrentDirectory(), @"Videos\");
        public static string GetDirectoryVideosPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryVideosPath);
                return DirectoryVideosPath;
            }
        }
        
        private static readonly string DirectoryBackgroundVideosPath = Path.Combine(Directory.GetCurrentDirectory(),@"Videos\Background\");
        public static string GetDirectoryBackgroundVideosPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryBackgroundVideosPath);
                return DirectoryBackgroundVideosPath;
            }
        }
        
        private static readonly string DirectoryGamesPath = Path.Combine(Directory.GetCurrentDirectory(), @"Games\");
        public static string GetDirectoryGamesPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryGamesPath);
                return DirectoryGamesPath;
            }
        }
        
        private static readonly string DirectoryCachePath = Path.Combine(Directory.GetCurrentDirectory(), @"Cache\");
        public static string GetDirectoryCachePath
        {
            get
            {
                Directory.CreateDirectory(DirectoryCachePath);
                return DirectoryCachePath;
            }
        }
        
        private static readonly string DirectoryTimeTablesPath = Path.Combine(Directory.GetCurrentDirectory(), @"TimeTables\");
        public static string GetDirectoryTimeTablesPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryTimeTablesPath);
                return DirectoryTimeTablesPath;
            }
        }

        private static readonly string DirectoryGestureDatabasePath = Path.Combine(Directory.GetCurrentDirectory(), @"Gesture\");
        public static string GetDirectoryGestureDatabasePath
        {
            get
            {
                Directory.CreateDirectory(DirectoryGestureDatabasePath);
                return DirectoryGestureDatabasePath;
            }
        }

        private static readonly string DirectoryEggPath = Path.Combine(Directory.GetCurrentDirectory(), @"vgbtechs\");
        public static string GetDirectoryEggPath
        {
            get
            {
                Directory.CreateDirectory(DirectoryEggPath);
                return DirectoryEggPath;
            }
        }

        private static readonly string DirectorySchedulePath =
            Path.Combine(Directory.GetCurrentDirectory(), "Images/ScheduleImages");

        public static string GetDirectorySchedulePath
        {
            get
            {
                Directory.CreateDirectory(DirectorySchedulePath);
                return DirectorySchedulePath;
            }
        }

        public static string GetDirectoryScheduleImages(DegreeEnum degreeTypeEnum)
        {
            var path = Path.Combine(GetDirectorySchedulePath, degreeTypeEnum.ToString());
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetFileScheduleImageForCourse(DegreeEnum? degreeTypeEnum, int? courseNumber)
        {
            if (courseNumber == null || degreeTypeEnum == null)
                throw new ArgumentException("CourseNumber or DegreeType can't be null");
            
            var directoryPath = GetDirectoryScheduleImages(degreeTypeEnum.Value);

            var filePath = Path.Combine(directoryPath, courseNumber.ToString()) + ".png";
            return filePath;
        }

        public static readonly string FileInactiveImageGerb = Path.Combine(Directory.GetCurrentDirectory(), "Images/Gerb.png");
        
        public const string FileConfigurationPath = "configuration.json";
        public const string FileLogsPath = "logs.txt";

        public static readonly string FileGroupsCachePath = GetDirectoryCachePath + "groups.json";
        public static readonly string FileNewsCachePath = GetDirectoryCachePath + "news.json";

        public static readonly string FileGestureDatabasePath = GetDirectoryGestureDatabasePath + "KinectGesture.gbd";

        public static readonly string FileEggVideoPath = GetDirectoryEggPath + "kinectrequired.mp4";
    }
}