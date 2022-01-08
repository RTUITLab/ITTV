using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITTV.WPF.Core.Helpers;

namespace ITTV.WPF.Core.Services
{
    public class VideosManager
    {
        public VideosManager()
        { }

        public IEnumerable<Uri> GetVideos()
        {
            var path = PathHelper.GetDirectoryVideosPath;
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