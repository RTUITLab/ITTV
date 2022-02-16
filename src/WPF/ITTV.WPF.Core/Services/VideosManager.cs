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
            return VideoHelper.GetSupportedVideoUris(path);
        }
    }
}