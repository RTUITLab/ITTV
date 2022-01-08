using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ITTV.WPF.MVVM.Services
{
    public class VideosManager
    {
        public VideosManager()
        { }

        public IEnumerable<Uri> GetVideos()
        {
            var path = AllPaths.GetDirectoryVideosPath;
            var files = Directory.GetFiles(path);

            var supportedVideoFormats = new[]
            {
                ".mov",
                ".mp4",
                ".ogg"
            };

            return files.Where(x => supportedVideoFormats.Contains(Path.GetExtension(x)))
                .Select(x => new Uri(x));
        }
    }
}