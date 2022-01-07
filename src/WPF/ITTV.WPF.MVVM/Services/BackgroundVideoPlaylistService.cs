using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITTV.WPF.MVVM.Helpers;
using ITTV.WPF.MVVM.Models;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.MVVM.Services
{
    public class BackgroundVideoPlaylistService
    {
        private int currentIndex = -1;
        private readonly List<Uri> _playlist = new();

        private Uri currentVideo;

        public BackgroundVideoPlaylistService(IOptions<Settings> settings)
        {
            var backgroundVideoDirectory = Path.Combine(Directory.GetCurrentDirectory(), AllPaths.GetDirectoryBackgroundVideosPath);

            var playlistFromSettings =
                BackgroundVideosHelper.FilteringByExistBackgroundVideos(settings.Value.BackgroundVideoOrder);
            foreach (var video in playlistFromSettings)
            {
                var uri = new Uri(Path.Combine(backgroundVideoDirectory, video));
                _playlist.Add(uri);
            }
            if (_playlist.Count > 0)
                currentVideo = _playlist.First();
        }
        
        public Uri NextVideo()
        {
            currentIndex = (currentIndex + 1) % _playlist.Count;

            currentVideo = new Uri(_playlist[currentIndex].AbsoluteUri);
            return currentVideo;
        }

        public bool ContainsAnyVideos()
            => _playlist.Count > 0;
    }
}
