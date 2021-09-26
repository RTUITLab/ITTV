using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    public class BackgroundVideoPlaylist
    {
        private int currentIndex;
        private readonly List<Uri> playlist = new List<Uri>();

        public Uri currentVideo;

        public BackgroundVideoPlaylist()
        {
            var test = Settings.Instance.BackgroundVideoOrder;
            foreach (var video in test)
            {
                playlist.Add(new Uri(video));
            }
            if (playlist.Count > 0)
                currentVideo = playlist.First();
        }
        
        public Uri NextVideo()
        {
            if (playlist.Count > currentIndex + 1)
                currentIndex++;
            else
                currentIndex = 0;

            currentVideo = playlist[currentIndex];

            return currentVideo;
        }
    }
}
