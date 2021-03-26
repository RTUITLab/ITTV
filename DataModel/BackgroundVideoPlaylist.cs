using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    class BackgroundVideoPlaylist
    {
        private int currentIndex = 0;
        private List<Uri> playlist = new List<Uri>();

        public Uri currentVideo;

        public BackgroundVideoPlaylist()
        {
            List<string> test = Settings.Instance.BackgroundVideoOrder;
            foreach (string video in test)
            {
                playlist.Add(new Uri(video));
            }
            if (playlist.Count > 0)
                currentVideo = playlist.First();
        }



        public Uri nextVideo()
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
