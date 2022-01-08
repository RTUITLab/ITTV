using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ITTV.WPF.MVVM.Helpers
{
    public static class BackgroundVideosHelper
    {
        public static IEnumerable<string> GetBackgroundVideos()
        {
            var supportedVideoFormats = new[]
            {
                ".mov", 
                ".ogg",
                ".mp4"
            };
            
            var fileNames = Directory.GetFiles(AllPaths.GetDirectoryBackgroundVideosPath)
                .Select(Path.GetFileName);
            
            var filteredFileNames = fileNames.Where(x =>
            {
                var fileExtension = Path.GetExtension(x);
                
                var fileSupported = supportedVideoFormats.Contains(fileExtension);
                if (!fileSupported)
                {
                    //TODO: Rewrite logger
                    //MainWindow.Log($"The format of the background video file {x} is not supported");
                }

                return fileSupported;
            });

            return filteredFileNames;
        }

        public static IEnumerable<string> FilteringByExistBackgroundVideos(IEnumerable<string> backgroundVideos)
        {
            if (backgroundVideos == null)
                return null;
            
            var existVideos = GetBackgroundVideos();
            return backgroundVideos.Where(x => existVideos.Contains(x));
        }
    }
}