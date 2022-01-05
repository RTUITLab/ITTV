using System;
using System.Collections.Generic;
using System.Linq;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.DataModel
{
    public class BackgroundVideoPlaylist
    {
        private int currentIndex;
        private readonly List<Uri> playlist = new List<Uri>();

        public Uri currentVideo;

        public BackgroundVideoPlaylist(Settings settings)
        {
            var test = settings.BackgroundVideoOrder;
            foreach (var video in test)
            {
                playlist.Add(new Uri(video));
            }
            if (playlist.Count > 0)
                currentVideo = playlist.First();
        }
        
        public Uri NextVideo()
        {
            currentIndex = (currentIndex + 1) % playlist.Count;

            currentVideo = playlist[currentIndex];
            return currentVideo;
        }
    }
}
