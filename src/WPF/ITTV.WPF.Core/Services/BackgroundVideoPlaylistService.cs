using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using Microsoft.Extensions.Options;

namespace ITTV.WPF.Core.Services
{
    public class BackgroundVideoPlaylistService
    {
        private int _currentIndex = -1;
        private readonly List<Uri> _playlist = new();

        private Uri _currentVideo;

        public BackgroundVideoPlaylistService(IOptions<Settings> settings)
        {
            var backgroundVideoDirectory = PathHelper.GetDirectoryBackgroundVideosPath;

            var path = PathHelper.GetDirectoryBackgroundVideosPath;
            
            var playlistFromSettings =
                VideoHelper.FilteringByExistVideos(settings.Value.BackgroundVideoOrder, path);
            foreach (var video in playlistFromSettings ?? Array.Empty<string>())
            {
                var uri = new Uri(Path.Combine(backgroundVideoDirectory, video));
                _playlist.Add(uri);
            }
            if (_playlist.Count > 0)
                _currentVideo = _playlist.First();
        }
        
        public Uri NextVideo()
        {
            _currentIndex = (_currentIndex + 1) % _playlist.Count;

            _currentVideo = new Uri(_playlist[_currentIndex].AbsoluteUri);
            return _currentVideo;
        }

        public bool ContainsAnyVideos()
            => _playlist.Count > 0;
    }
}
