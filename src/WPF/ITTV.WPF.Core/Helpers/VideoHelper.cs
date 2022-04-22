using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

namespace ITTV.WPF.Core.Helpers
{
    public static class VideoHelper
    {
        public static IEnumerable<Uri> GetSupportedVideoUris(string path)
        {
            var videos = GetSupportedVideos(path);
            return videos.Select(x => new Uri(Path.Combine(path, x)));
        }
        public static IEnumerable<string> GetSupportedVideos(string path)
        {
            var supportedVideoFormats = new[]
            {
                ".mov", 
                ".ogg",
                ".mp4"
            };
            
            var fileNames = Directory.GetFiles(path)
                .Select(Path.GetFileName);
            
            var filteredFileNames = fileNames.Where(x =>
            {
                var fileExtension = Path.GetExtension(x);
                
                var fileSupported = supportedVideoFormats.Contains(fileExtension);
                if (!fileSupported)
                {
                    Log.Logger.Warning("{0}: The format of the background video file {1} is not supported",
                        nameof(VideoHelper), x);
                }

                return fileSupported;
            });

            return filteredFileNames;
        }

        public static IEnumerable<string> FilteringByExistVideos(IEnumerable<string> backgroundVideos, string path)
        {
            if (backgroundVideos == null)
                return null;
            
            var existVideos = GetSupportedVideos(path);
            return backgroundVideos.Where(x => existVideos.Contains(x));
        }
    }
}